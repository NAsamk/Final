using System;

namespace BankApp.Models
{
    internal class Transaction
    {
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }

        public Transaction(string type, decimal amount, string currency, string description = "")
        {
            TransactionDate = DateTime.Now;
            Type = type;
            Amount = amount;
            Currency = currency;
            Description = description;
        }

        public override string ToString()
        {
            return $"{TransactionDate:G} | {Type} | {Amount:F2} {Currency} | {Description}";
        }
    }
}
