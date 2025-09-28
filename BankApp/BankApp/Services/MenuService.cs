using System;
using System.Collections.Generic;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class MenuService
    {
        public void Show(Cardinfo user, List<Cardinfo> users, string filePath)
        {
            var accountService = new AccountService();
            var securityService = new SecurityService();
            var currencyConversion = new CurrencyConversion();
            var bankServices = new BankServices();

            bool exit = false;

            while (!exit)
            {
                try
                {
                    Console.WriteLine("\n--- Menu ---");
                    Console.WriteLine("1. Show Balance");
                    Console.WriteLine("2. Withdraw Money");
                    Console.WriteLine("3. Deposit Money");
                    Console.WriteLine("4. Show Transactions");
                    Console.WriteLine("5. Change PIN");
                    Console.WriteLine("6. Currency Conversion");
                    Console.WriteLine("0. Exit");
                    Console.Write("Choose option: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            accountService.ShowBalance(user);
                            break;
                        case "2":
                            if (accountService.Withdraw(user, users))
                                FileService.SaveUsers(users, filePath);
                            break;
                        case "3":
                            if (accountService.Deposit(user, users))
                                FileService.SaveUsers(users, filePath);
                            break;
                        case "4":
                            bankServices.ShowTransactionHistory(user);
                            break;
                        case "5":
                            if (securityService.ChangePin(user, users, filePath))
                                Console.WriteLine("PIN updated.");
                            break;
                        case "6":
                            currencyConversion.ConvertCurrency(user, filePath);
                            break;
                        case "0":
                            exit = true;
                            Console.WriteLine("Goodbye.");
                            break;
                        default:
                            Console.WriteLine("Invalid choice.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex);
                    Console.WriteLine("An error occurred.");
                }
            }
        }
    }
}
