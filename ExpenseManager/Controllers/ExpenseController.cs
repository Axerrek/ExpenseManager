using ExpenseManager.Data;
using ExpenseManager.Models;
using ExpenseManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace ExpenseManager.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NbpApiService _nbpApiService;

        public interface INbpApiService
        {
            Task<decimal?> GetExchangeRateAsync(Currency currency, DateTime? date = null);
        }
        public ExpensesController(ApplicationDbContext context, NbpApiService nbpApiService)
        {
            _context = context;
            _nbpApiService = nbpApiService;
        }

        public async Task<IActionResult> Index(string searchString, ExpenseCategory? category)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = _context.Expenses.Where(e => e.UserId == userId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.Description.ToLower().Contains(searchString.ToLower()));
            }

            if (category.HasValue)
            {
                query = query.Where(e => e.Category == category.Value);
            }

            var expenses = await query
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            ViewData["TotalSum"] = expenses.Sum(e => e.Price);

            ViewData["CurrentSearch"] = searchString;
            ViewData["CurrentCategory"] = category;

            ViewBag.CategoryList = new SelectList(
                Enum.GetValues(typeof(ExpenseCategory))
                    .Cast<ExpenseCategory>()
                    .Select(e => new { Id = e, Name = e.ToString() }),
                "Id",
                "Name",
                category 
                );

            return View(expenses);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            expense.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ModelState.Remove(nameof(expense.UserId));
            ModelState.Remove(nameof(expense.Price));
            ModelState.Remove(nameof(expense.ExchangeRate));

            if (ModelState.IsValid)
            {
                // Pobranie kursu z NBPAPI i wyliczenie
                var rate = await _nbpApiService.GetExchangeRateAsync(expense.Currency);
                expense.ExchangeRate = rate ?? 1.0m;
                expense.Price = Math.Round(expense.OriginalPrice * expense.ExchangeRate, 2);

                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var expense = await _context.Expenses
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Expense expense)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            expense.UserId = userId;

            if (ModelState.IsValid)
            {
                _context.Update(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }

    }
}