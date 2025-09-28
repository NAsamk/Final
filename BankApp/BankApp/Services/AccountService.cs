using System;
using System.Collections.Generic;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class AccountService
    {
        public void ShowBalance(Cardinfo user)
        {
            try
            {
                Console.WriteLine("\n--- Current Balances ---");
                foreach (var balance in user.Balances)
                {
                    Console.WriteLine($"{balance.Key}: {balance.Value:F2}");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Could not display balance.");
            }
        }

        public bool Withdraw(Cardinfo user, List<Cardinfo> users)
        {
            try
            {
                Console.Write("Enter amount to withdraw (GEL): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return false;
                }

                if (amount > user.Balances["GEL"])
                {
                    Console.WriteLine("Insufficient funds.");
                    return false;
                }

                user.Balances["GEL"] -= amount;
                user.Transactions.Add(new Transaction("Withdraw", amount, "GEL", "Cash withdrawal"));

                Console.WriteLine($"New GEL balance: {user.Balances["GEL"]:F2}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Withdrawal failed.");
                return false;
            }
        }

        public bool Deposit(Cardinfo user, List<Cardinfo> users)
        {
            try
            {
                Console.Write("Enter deposit amount (GEL): ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount) || amount <= 0)
                {
                    Console.WriteLine("Invalid amount.");
                    return false;
                }

                user.Balances["GEL"] += amount;
                user.Transactions.Add(new Transaction("Deposit", amount, "GEL", "Cash deposit"));

                Console.WriteLine($"New GEL balance: {user.Balances["GEL"]:F2}");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Deposit failed.");
                return false;
            }
        }
    }
}
