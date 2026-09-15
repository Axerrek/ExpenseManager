using ExpenseManager.Data;
using ExpenseManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpensesController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(string searchString, ExpenseCategory? category)
        {
            var query = _context.Expenses.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(e => e.Description.ToLower().Contains(searchString.ToLower()));
            }

            if (category.HasValue)
            {
                query = query.Where(e => e.Category == category.Value);
            }

            ViewData["TotalSum"] = await query.SumAsync(e => (decimal?)e.Price) ?? 0m;

            var expenses = await query
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _context.Add(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(expense);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
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
            if (ModelState.IsValid)
            {
                _context.Update(expense);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(expense);
        }

        public async Task<IActionResult> Analytics()
        {
            var categoryData = await _context.Expenses
                .GroupBy(e => e.Category)
                .Select(g => new
                {
                    Category = g.Key.ToString(),
                    Total = g.Sum(e => e.Price ?? 0m)
                })
                .ToListAsync();

            ViewBag.CategoryLabels = categoryData.Select(d => d.Category).ToArray();
            ViewBag.CategoryTotals = categoryData.Select(d => d.Total).ToArray();

            var monthlyData = await _context.Expenses
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(e => e.Price ?? 0m)
                })
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ToListAsync();

            ViewBag.MonthlyLabels = monthlyData
                .Select(d => $"{d.Month:D2}.{d.Year}")
                .ToArray();

            ViewBag.MonthlyTotals = monthlyData
                .Select(d => d.Total)
                .ToArray();

            return View();
        }

    }
}