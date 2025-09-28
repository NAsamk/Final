using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class LoginService
    {
        private const int MaxPinAttempts = 3;

        public Cardinfo AuthenticateUser(List<Cardinfo> users)
        {
            try
            {
                Console.Write("Enter Card Number: ");
                string rawNumber = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter Expiry (MM/YY): ");
                string rawExpiry = Console.ReadLine() ?? string.Empty;

                string inputNumber = NormalizeCardNumber(rawNumber);
                string inputExpiry = FormatExpiryDate(rawExpiry);

                if (string.IsNullOrWhiteSpace(inputNumber) || string.IsNullOrWhiteSpace(inputExpiry))
                {
                    Console.WriteLine("Invalid card number or expiry format.");
                    return null;
                }

                var user = users.FirstOrDefault(c =>
                    NormalizeCardNumber(c.CardNumber) == inputNumber &&
                    FormatExpiryDate(c.ExpiryDate) == inputExpiry
                );

                if (user == null)
                {
                    Console.WriteLine("Card not found.");
                    return null;
                }

                if (IsCardExpired(user.ExpiryDate))
                {
                    Console.WriteLine("Card has expired.");
                    return null;
                }

                for (int attempt = 1; attempt <= MaxPinAttempts; attempt++)
                {
                    Console.Write("Enter PIN: ");
                    string inputPin = Console.ReadLine() ?? string.Empty;

                    if (!int.TryParse(inputPin, out int pin))
                    {
                        Console.WriteLine("PIN must be numeric.");
                    }
                    else if (user.Pin == pin)
                    {
                        return user;
                    }
                    else
                    {
                        if (attempt < MaxPinAttempts)
                            Console.WriteLine($"Incorrect PIN. {MaxPinAttempts - attempt} attempt(s) left.");
                    }
                }

                Console.WriteLine("Too many incorrect PIN attempts. Access denied.");
                Logger.LogError(new Exception($"Card {inputNumber} blocked due to too many incorrect PIN attempts."));
                return null;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Login failed.");
                return null;
            }
        }

        private string NormalizeCardNumber(string input)
        {
            try
            {
                if (string.IsNullOrEmpty(input)) return string.Empty;
                return new string(input.Where(char.IsDigit).ToArray());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return input ?? string.Empty;
            }
        }

        private string FormatExpiryDate(string input)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(input)) return string.Empty;

                var digits = input.Where(char.IsDigit).ToArray();

                if (digits.Length == 4) // MMYY -> MM/YY
                    return $"{new string(digits.Take(2).ToArray())}/{new string(digits.Skip(2).ToArray())}";

                if (digits.Length == 6) // MMYYYY -> MM/YYYY -> normalize to MM/YY
                {
                    string mm = new string(digits.Take(2).ToArray());
                    string yyyy = new string(digits.Skip(2).ToArray());
                    if (int.TryParse(yyyy, out int y) && yyyy.Length == 4)
                        return $"{mm}/{(y % 100):D2}";
                }

                
                if (DateTime.TryParse(input, out DateTime dt))
                {
                    return dt.ToString("MM/yy");
                }

                return input.Trim();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return input ?? string.Empty;
            }
        }

        private bool IsCardExpired(string expiryDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(expiryDate))
                    return true;

                string[] formats = { "MM/yy", "MM/yyyy", "M/yy", "M/yyyy" };
                if (DateTime.TryParseExact(expiryDate, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsed))
                {
                    int year = parsed.Year;
                    int month = parsed.Month;
                    var lastDayOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    return lastDayOfMonth < DateTime.Today;
                }

                // Fallback: try to extract MM and YY/ YYYY from digits
                var digits = expiryDate.Where(char.IsDigit).ToArray();
                if (digits.Length == 4)
                {
                    int month = int.Parse(new string(digits.Take(2).ToArray()));
                    int yearTwo = int.Parse(new string(digits.Skip(2).ToArray()));
                    int year = 2000 + yearTwo;
                    var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    return lastDay < DateTime.Today;
                }
                if (digits.Length == 6)
                {
                    int month = int.Parse(new string(digits.Take(2).ToArray()));
                    int year = int.Parse(new string(digits.Skip(2).ToArray()));
                    var lastDay = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    return lastDay < DateTime.Today;
                }

                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return true;
            }
        }
    }
}
