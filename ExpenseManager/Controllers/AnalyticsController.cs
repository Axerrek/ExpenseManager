using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseManager.Data;

namespace ExpenseManager.Controllers
{
    [Authorize]
    public class AnalyticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            // Wykres kołowy - podział na kategorie
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


            // wykres słupkowy - wydatki z poszczególnych miesięcy
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
