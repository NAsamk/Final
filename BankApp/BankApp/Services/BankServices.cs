using System;
using System.Linq;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class BankServices
    {
        public void ShowTransactionHistory(Cardinfo user)
        {
            try
            {
                if (user.Transactions.Count == 0)
                {
                    Console.WriteLine("No transactions found.");
                    return;
                }

                Console.WriteLine("Last 5 transactions:");
                var last5 = user.Transactions
                    .OrderByDescending(t => t.TransactionDate)
                    .Take(5);

                foreach (var t in last5)
                {
                    Console.WriteLine(t.ToString());
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Could not load transaction history.");
            }
        }
    }
}
