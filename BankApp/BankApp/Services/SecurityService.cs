using System;
using System.Collections.Generic;
using System.Linq;
using BankApp.Models;
using BankApp.Utilities;

namespace BankApp.Services
{
    internal class SecurityService
    {
        public bool ChangePin(Cardinfo user, List<Cardinfo> users, string filePath)
        {
            try
            {
                Console.Write("Enter current PIN: ");
                string oldPin = Console.ReadLine();

                if (oldPin != user.Pin.ToString())
                {
                    Console.WriteLine("Incorrect current PIN.");
                    return false;
                }

                Console.Write("Enter new PIN: ");
                string newPin1 = Console.ReadLine();

                Console.Write("Confirm new PIN: ");
                string newPin2 = Console.ReadLine();

                if (newPin1 != newPin2 || newPin1.Length != 4 || !newPin1.All(char.IsDigit))
                {
                    Console.WriteLine("Invalid new PIN.");
                    return false;
                }

                user.Pin = int.Parse(newPin1);
                FileService.SaveUsers(users, filePath);

                Console.WriteLine("PIN changed successfully.");
                return true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Failed to change PIN.");
                return false;
            }
        }
    }
}
