using BankApp;
using System;
using BankApp.Utilities;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            var app = new App();
            app.Run();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex);
            Console.WriteLine("Unexpected application error.");
        }
    }
}
