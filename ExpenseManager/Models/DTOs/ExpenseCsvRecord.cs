using CsvHelper.Configuration.Attributes;

namespace ExpenseManager.Models.DTOs;

public class ExpenseCsvRecord
{
    [Name("Tytuł")]
    public string Title { get; set; } = string.Empty;

    [Name("Kwota")]
    public decimal OriginalPrice { get; set; }

    [Name("Waluta")]
    public Currency Currency { get; set; } = Currency.PLN;

    [Name("Kategoria")]
    public string Category { get; set; } = string.Empty;

    [Name("Data")]
    public DateTime Date { get; set; }
}