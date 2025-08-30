using Assignment.Configuration;
using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Extensions.Options;
using System.Runtime;
using System.Text.Json;

namespace Assignment.Services
{
    public class OpenWeatherService : IOpenWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherSettings _settings;
        private readonly ILogger<OpenWeatherService> _logger;

        public OpenWeatherService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            ILogger<OpenWeatherService> logger)
        {
            _httpClient = httpClient;
            _settings = apiSettings.Value.ExternalApis.OpenWeather;
            _logger = logger;

            if (!string.IsNullOrWhiteSpace(_settings.BaseUrl)) { 
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            }
        }

        public async Task<List<OpenWeatherDTO>> GetOpenWeatherAsync(AggregationQuery query)
        {
            var results = new List<OpenWeatherDTO>();

            try
            {

                var requestUrl = $"data/2.5/forecast?q={Uri.EscapeDataString(query.City)},{query.Country}" +
                         $"&appid={_settings.ApiKey}&units=metric&cnt={query.PageSize}";

                var response = await _httpClient.GetAsync(requestUrl);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Could not retrieve weather forecast");

                var json = await response.Content.ReadAsStringAsync();
                var forecastResponse = JsonSerializer.Deserialize<OpenWeatherResponse>(json);

                if (forecastResponse == null || forecastResponse.List == null)
                    return new List<OpenWeatherDTO>();

                foreach (ForecastItem item in forecastResponse.List)
                {
                    results.Add(MapToDto(item, forecastResponse.City));
                }
                //LINQ version
                //var results = forecastResponse.List.Select(item => MapToDto(item, forecastResponse.City)).ToList();

                // Filtering on pulled data
                if (query.FromDate.HasValue)
                    results = results.Where(r => r.Timestamp >= query.FromDate.Value).ToList();

                if (query.ToDate.HasValue)
                    results = results.Where(r => r.Timestamp <= query.ToDate.Value).ToList();

                if (!string.IsNullOrEmpty(query.WeatherCondition))
                    results = results.Where(r => r.Condition.Contains(query.WeatherCondition, StringComparison.OrdinalIgnoreCase)).ToList();

                if (query.MinTemperature.HasValue)
                    results = results.Where(r => r.Temperature >= query.MinTemperature.Value).ToList();

                if (query.MaxTemperature.HasValue)
                    results = results.Where(r => r.Temperature <= query.MaxTemperature.Value).ToList();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Weather service failed");
                return new List<OpenWeatherDTO>();
            }
            return results;
        }

        private static OpenWeatherDTO MapToDto(ForecastItem item, ForecastCity city)
        {
            return new OpenWeatherDTO
            {
                City = city.Name,
                Country = city.Country,
                Temperature = item.Main.Temp,
                FeelsLike = item.Main.FeelsLike,
                Humidity = item.Main.Humidity,
                WindSpeed = item.Wind.Speed,
                Description = item.Weather.FirstOrDefault()?.Description ?? string.Empty,
                Condition = item.Weather.FirstOrDefault()?.Main ?? string.Empty,
                Timestamp = DateTime.Parse(item.DtTxt)
            };
        }
    }
}
