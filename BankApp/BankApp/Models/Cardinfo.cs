using System;
using System.Collections.Generic;

namespace BankApp.Models
{
    internal class Cardinfo
    {
        public string CardNumber { get; set; }
        public string ExpiryDate { get; set; }
        public int Pin { get; set; }
        public string Currency { get; set; } = "GEL";


        public Dictionary<string, decimal> Balances { get; set; } = new Dictionary<string, decimal>
        {
            { "GEL", 0 },
            { "USD", 0 },
            { "EUR", 0 }
        };

        public List<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Cardinfo() { }

        public Cardinfo(string cardNumber, string expiryDate, int pin)
        {
            CardNumber = cardNumber;
            ExpiryDate = expiryDate;
            Pin = pin;
        }
    }
}
