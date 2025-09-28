using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class CurrencyConversion
    {
        private readonly Dictionary<string, decimal> rates = new()
        {
            { "GEL", 1m },
            { "USD", 2.7m },
            { "EUR", 3.0m }
        };

        public void ConvertCurrency(Cardinfo user, string filePath)
        {
            try
            {
                // არსებული ბალანსების ჩვენება
                Console.WriteLine("Balances:");
                user.Balances.ToList().ForEach(b =>
                    Console.WriteLine($"{b.Key}: {b.Value:F2}"));

                // From currency
                string from = ChooseCurrency("Choose currency to convert FROM:", "");
                string to = ChooseCurrency("Choose currency to convert TO:", from);

                if (string.IsNullOrEmpty(from) || string.IsNullOrEmpty(to))
                {
                    Console.WriteLine("Invalid currency selection.");
                    return;
                }

                Console.Write("Enter amount to convert: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return;
                }

                if (user.Balances[from] < amount)
                {
                    Console.WriteLine("Insufficient funds.");
                    return;
                }

                // GEL-ში გადაყვანა და შემდეგ მიზნობრივ ვალუტაში კონვერტაცია
                decimal gelAmount = amount * rates[from];
                decimal result = gelAmount / rates[to];

                // ანგარიშების განახლება
                user.Balances[from] -= amount;
                user.Balances[to] += Math.Round(result, 2);

                user.Transactions.Add(
                    new Transaction("Currency Conversion", amount, from, $"Converted to {result:F2} {to}")
                );

                // JSON-ში შენახვა
                var users = FileService.LoadUsers(filePath);
                users.Where(x => x.CardNumber == user.CardNumber)
                     .ToList()
                     .ForEach(u =>
                     {
                         u.Balances = user.Balances;
                         u.Transactions = user.Transactions;
                     });

                FileService.SaveUsers(users, filePath);
                Console.WriteLine("Conversion complete.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Currency conversion failed.");
            }
        }

        private string ChooseCurrency(string message, string exclude)
        {
            Console.WriteLine($"\n{message}");

            var currencies = rates.Keys
                .Where(c => c != exclude)
                .Select((c, i) => new { Index = i + 1, Code = c })
                .ToList();

            currencies.ForEach(c => Console.WriteLine($"{c.Index}. {c.Code}"));

            string choice = Console.ReadLine() ?? "";
            return currencies.FirstOrDefault(c => c.Index.ToString() == choice)?.Code ?? "";
        }
    }
}
