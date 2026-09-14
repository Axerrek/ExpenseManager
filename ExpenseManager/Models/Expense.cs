namespace ExpenseManager.Models
{

    public enum ExpenseCategory
{
        Jedzenie,
        Dom,
        Rozrywka,
}
    public class Expense
    {
        public int Id { get; set; }
        public ExpenseCategory Category { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

    }
}
