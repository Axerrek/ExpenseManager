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
            new Expense { Description = "Rachunek za pr¹d", Price = 220.00m, Category = ExpenseCategory.Dom, Date = DateTime.Now }
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