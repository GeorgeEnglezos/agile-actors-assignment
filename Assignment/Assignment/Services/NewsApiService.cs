using Assignment.Configuration;
using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace Assignment.Services
{
    public class NewsApiService : INewsApiService
    {
        private readonly HttpClient _httpClient;
        private readonly NewsApiSettings _settings;
        private readonly ILogger<NewsApiService> _logger;

        public NewsApiService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            ILogger<NewsApiService> logger)
        {
            _httpClient = httpClient;
            _settings = apiSettings.Value.ExternalApis.NewsApi;
            _logger = logger;

            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AgileActorsAssignment", "1.0"));
            if (!string.IsNullOrWhiteSpace(_settings.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            }
        }

        public async Task<NewsApiFullDto> GetNewsAsync(AggregationQuery query)
        {
            NewsApiFullDto newsResult = new NewsApiFullDto();
            try
            {
                var url = $"everything?q={query.City}+{query.NewsKeyword}&sortBy={query.NewsSortBy}&pageSize={query.PageSize}&apiKey={_settings.ApiKey}";

                if (query.FromDate.HasValue)
                    url += $"&from={query.FromDate.Value:yyyy-MM-dd}";
                if (query.ToDate.HasValue)
                    url += $"&to={query.ToDate.Value:yyyy-MM-dd}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new Exception($"NewsAPI request failed: {response.StatusCode} - {error}");
                }

                var json = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<NewsApiFullResponse>(json);

                if (apiResponse == null)
                    throw new Exception("Failed to parse NewsAPI response");

                newsResult = MapToDto(apiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "News service failed");
                return new NewsApiFullDto { TotalResults = 0, Articles = new List<NewsApiDto>() };
            }
            return newsResult;
        }


        private static NewsApiFullDto MapToDto(NewsApiFullResponse response)
        {
            NewsApiFullDto dtoObject = new NewsApiFullDto
            {
                TotalResults = response.TotalResults,
                Articles = new List<NewsApiDto>()
            };

            foreach (var art in response.Articles) {
                dtoObject.Articles.Add( new NewsApiDto{
                    Author = art.Author,
                    Title = art.Title,
                    Description = art.Description,
                    Url = art.Url,
                    UrlToImage = art.UrlToImage,
                    PublishedAt = art.PublishedAt,
                    Source = new SourceDto {
                        name = art.Source.name
                    }
                });
            }

            return dtoObject;
        }
    }
}