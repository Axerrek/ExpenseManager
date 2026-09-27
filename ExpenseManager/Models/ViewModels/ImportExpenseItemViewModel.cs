using System;

namespace ExpenseManager.Models.ViewModels
{
    public class ImportExpenseItemViewModel
    {
        public bool Selected { get; set; } = true;

        public DateTime Date { get; set; } = DateTime.Now;

        public string Description { get; set; } = string.Empty;

        public decimal OriginalPrice { get; set; }

        public Currency Currency { get; set; } = Currency.PLN;

        public decimal ExchangeRate { get; set; } = 1.0m;

        public decimal Price { get; set; }

        public ExpenseCategory Category { get; set; } = ExpenseCategory.Inne;
    }
}