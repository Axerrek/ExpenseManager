using ExpenseManager.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Expense> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Expense>().HasData(
                new Expense
                {
                    Id = 1,
                    Description = "Zakupy spożywcze w Biedronce",
                    Price = 145.80m,
                    Category = ExpenseCategory.Jedzenie,
                    Date = new DateTime(2024, 1, 15)
                },
                new Expense
                {
                    Id = 2,
                    Description = "Rachunek za prąd",
                    Price = 230.00m,
                    Category = ExpenseCategory.Dom,
                    Date = new DateTime(2024, 1, 18)
                },
                new Expense
                {
                    Id = 3,
                    Description = "Bilet do kina",
                    Price = 35.50m,
                    Category = ExpenseCategory.Rozrywka,
                    Date = new DateTime(2024, 1, 20)
                }
            );
        }
    }
}