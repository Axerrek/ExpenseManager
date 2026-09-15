using ExpenseManager.Data;
using ExpenseManager.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("ExpenseTrackerDb"));

var app = builder.Build();

// dane testowe
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    if (!context.Expenses.Any())
    {
        context.Expenses.AddRange(
        new Expense { Description = "Zakupy spo¿ywcze", Price = 150.50m, Category = ExpenseCategory.Jedzenie, Date = DateTime.Now.AddDays(-2) },
        new Expense { Description = "Bilet do kina", Price = 35.00m, Category = ExpenseCategory.Rozrywka, Date = DateTime.Now.AddDays(-1) },
        new Expense { Description = "Rachunek za pr¹d", Price = 220.00m, Category = ExpenseCategory.Dom, Date = DateTime.Now },
        new Expense { Description = "Abonament Netflix", Price = 43.00m, Category = ExpenseCategory.Rozrywka, Date = DateTime.Now.AddDays(-5) },
        new Expense { Description = "Obiad w restauracji", Price = 112.50m, Category = ExpenseCategory.Jedzenie, Date = DateTime.Now.AddDays(-7) },
        new Expense { Description = "Internet i telewizja", Price = 89.90m, Category = ExpenseCategory.Dom, Date = DateTime.Now.AddDays(-10) },
        new Expense { Description = "Kawa na mieœcie", Price = 18.50m, Category = ExpenseCategory.Jedzenie, Date = DateTime.Now.AddDays(-14) },
        new Expense { Description = "Rachunek za gaz", Price = 185.20m, Category = ExpenseCategory.Dom, Date = DateTime.Now.AddDays(-35) },
        new Expense { Description = "Zamówienie z Pyszne.pl", Price = 65.00m, Category = ExpenseCategory.Jedzenie, Date = DateTime.Now.AddDays(-50) },
        new Expense { Description = "Bilet na koncert", Price = 180.00m, Category = ExpenseCategory.Rozrywka, Date = DateTime.Now.AddDays(-65) }
);
        context.SaveChanges();
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Expenses}/{action=Index}/{id?}");

app.Run();