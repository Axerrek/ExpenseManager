using ExpenseManager.Data;
using ExpenseManager.Models;
using ExpenseManager.Models.ViewModels;
using ExpenseManager.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Security.Claims;

namespace ExpenseManager.Controllers
{
    public class ImportController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly NbpApiService _nbpApiService;

        public ImportController(ApplicationDbContext context, NbpApiService nbpApiService)
        {
            _context = context;
            _nbpApiService = nbpApiService;
        }

        [HttpGet]
        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile csvFile)
        {
            if (csvFile == null || csvFile.Length == 0)
            {
                ModelState.AddModelError("", "Proszę wybrać plik CSV do importu.");
                return View();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var itemsToPreview = new List<ImportExpenseItemViewModel>();

            using (var reader = new StreamReader(csvFile.OpenReadStream()))
            {
                string? line;
                char separator = ';';
                bool headerFound = false;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    if (!headerFound)
                    {
                        if (line.Contains("Started Date") && line.Contains("Amount"))
                        {
                            separator = ','; // Revolut
                            headerFound = true;
                            continue;
                        }
                        else if (line.Contains("#Data operacji") || line.Contains("Data operacji"))
                        {
                            separator = ';'; // mBank
                            headerFound = true;
                            continue;
                        }
                        continue;
                    }

                    var parts = line.Split(separator);

                    if (separator == ',')
                    {
                        // --- PARSER REVOLUT ---
                        if (parts.Length >= 9 && parts[8].Trim() == "COMPLETED")
                        {
                            string rawAmount = parts[5].Trim();
                            if (decimal.TryParse(rawAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount < 0)
                            {
                                if (DateTime.TryParse(parts[2], CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                                {
                                    var currency = Enum.TryParse<Currency>(parts[7], true, out var parsedCurr) ? parsedCurr : Currency.PLN;
                                    var absoluteAmount = Math.Abs(amount);
                                    var rate = await _nbpApiService.GetExchangeRateAsync(currency, date) ?? 1.0m;

                                    itemsToPreview.Add(new ImportExpenseItemViewModel
                                    {
                                        Selected = true,
                                        Description = parts[4].Replace("\"", "").Trim(),
                                        OriginalPrice = absoluteAmount,
                                        Currency = currency,
                                        ExchangeRate = rate,
                                        Price = Math.Round(absoluteAmount * rate, 2),
                                        Category = ExpenseCategory.Inne,
                                        Date = date
                                    });
                                }
                            }
                        }
                    }
                    else if (separator == ';')
                    {
                        // --- PARSER mBANK ---
                        if (parts.Length >= 8)
                        {
                            string rawAmount = parts[6].Replace(" ", "").Replace(",", ".").Trim();
                            if (decimal.TryParse(rawAmount, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount) && amount < 0)
                            {
                                if (DateTime.TryParse(parts[0], out var date))
                                {
                                    Enum.TryParse<Currency>(parts[7], true, out var currency);
                                    var absoluteAmount = Math.Abs(amount);
                                    var rate = await _nbpApiService.GetExchangeRateAsync(currency, date) ?? 1.0m;

                                    itemsToPreview.Add(new ImportExpenseItemViewModel
                                    {
                                        Selected = true,
                                        Description = parts[3].Replace("\"", "").Trim(),
                                        OriginalPrice = absoluteAmount,
                                        Currency = currency,
                                        ExchangeRate = rate,
                                        Price = Math.Round(absoluteAmount * rate, 2),
                                        Category = ExpenseCategory.Inne,
                                        Date = date
                                    });
                                }
                            }
                        }
                    }
                }
            }

            if (itemsToPreview.Any())
            {
                return View("Preview", itemsToPreview);
            }

            ModelState.AddModelError("", "Nie znaleziono żadnych obciążeń (wydatków) w przesłanym pliku CSV.");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveImported(List<ImportExpenseItemViewModel> items)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            var selectedItems = items.Where(x => x.Selected).ToList();

            if (!selectedItems.Any())
            {
                TempData["ErrorMessage"] = "Nie zaznaczono żadnych wydatków do zapisu.";
                return RedirectToAction("Import");
            }

            var expensesToAdd = selectedItems.Select(item => new Expense
            {
                Date = item.Date,
                Description = item.Description.Length > 100 ? item.Description.Substring(0, 100) : item.Description,
                OriginalPrice = item.OriginalPrice,
                Currency = item.Currency,
                ExchangeRate = item.ExchangeRate,
                Price = item.Price,
                Category = item.Category,
                UserId = userId
            }).ToList();

            _context.Expenses.AddRange(expensesToAdd);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Pomyślnie zaimportowano {expensesToAdd.Count} wydatków!";
            return RedirectToAction("Index", "Expenses");
        }
    }
}