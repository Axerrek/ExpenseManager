using ExpenseManager.Data;
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

        // GET: /Expenses
        public async Task<IActionResult> Index()
        {
            var expenses = await _context.Expenses
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            return View(expenses);
        }
    }
}