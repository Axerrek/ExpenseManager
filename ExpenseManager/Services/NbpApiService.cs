using ExpenseManager.Models;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace ExpenseManager.Services;

public class NbpApiService
{
    private readonly HttpClient _httpClient;

    public NbpApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal?> GetExchangeRateAsync(Currency currency, DateTime? date = null)
    {
        if (currency == Currency.PLN)
        {
            return 1.0m;
        }

        var currencyCode = currency.ToString().ToLower();

        if (!date.HasValue)
        {
            return await FetchRateFromUrlAsync($"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode}/?format=json");
        }

        var targetDate = date.Value;

        for (int i = 0; i < 7; i++)
        {
            string formattedDate = targetDate.ToString("yyyy-MM-dd");
            string url = $"http://api.nbp.pl/api/exchangerates/rates/a/{currencyCode}/{formattedDate}/?format=json";

            var rate = await FetchRateFromUrlAsync(url);
            if (rate.HasValue)
            {
                return rate.Value;
            }

            targetDate = targetDate.AddDays(-1);
        }

        return null;
    }

    private async Task<decimal?> FetchRateFromUrlAsync(string url)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<NbpExchangeRateResponse>(url);
            return response?.Rates?.FirstOrDefault()?.Mid;
        }
        catch
        {
            return null;
        }
    }
}

public class NbpExchangeRateResponse
{
    [JsonPropertyName("rates")]
    public List<NbpRate>? Rates { get; set; }
}

public class NbpRate
{
    [JsonPropertyName("mid")]
    public decimal Mid { get; set; }
}