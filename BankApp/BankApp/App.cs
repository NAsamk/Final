using System;
using System.Collections.Generic;
using System.IO;
using BankApp.Models;
using BankApp.Services;
using BankApp.Utilities;

namespace BankApp
{
    internal class App
    {
        private const string FilePath = "C:\\Users\\User\\Desktop\\homework-8\\BankApp\\BankApp\\Data\\users.json";
        private List<Cardinfo> users;

        public void Run()
        {
            try
            {
                EnsureDataDirectoryExists();

                users = FileService.LoadUsers(FilePath);

                if (users == null || users.Count == 0)
                {
                    Console.WriteLine("No users found.");
                    return;
                }

               
                foreach (var c in users)
                {
                    c.Balances ??= new Dictionary<string, decimal>
                    {
                        { "GEL", 0 },
                        { "USD", 0 },
                        { "EUR", 0 }
                    };

                    c.Transactions ??= new List<Transaction>();
                }

                var loginService = new LoginService();
                Cardinfo user = null;

                while (user == null)
                {
                    user = loginService.AuthenticateUser(users);

                    if (user == null)
                    {
                        Console.WriteLine("\nInvalid credentials. Press ENTER to try again or press Q to quit.");
                        string retry = Console.ReadLine();

                        if (retry?.ToUpper() == "Q")
                        {
                            Console.WriteLine("Goodbye!");
                            return;
                        }
                    }
                }

                var menu = new MenuService();
                menu.Show(user, users, FilePath);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Application error. Please try again later.");
            }
        }

        private void EnsureDataDirectoryExists()
        {
            try
            {
                var directory = Path.GetDirectoryName(FilePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Could not create data directory.");
                throw;
            }
        }
    }
}
