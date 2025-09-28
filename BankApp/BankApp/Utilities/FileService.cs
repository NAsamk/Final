using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using BankApp.Models;

namespace BankApp.Utilities
{
    internal static class FileService
    {
        public static void SaveUsers(List<Cardinfo> users, string filePath)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(users, options);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Failed to save user data.");
            }
        }

        public static List<Cardinfo> LoadUsers(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return new List<Cardinfo>();

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Cardinfo>>(json);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                Console.WriteLine("Failed to load user data.");
                return new List<Cardinfo>();
            }
        }
    }
}
