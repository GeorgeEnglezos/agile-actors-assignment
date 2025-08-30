using Assignment.Controllers;
using Assignment.Interfaces;
using Assignment.Models;

namespace Assignment.Services
{
    public class AggregatedDataService : IAggregatedDataService
    {
        private readonly IOpenWeatherService _openWeatherService;
        private readonly IGithubApiService _githubApiService;
        private readonly INewsApiService _newsApiService;
        private readonly ILogger<AggregatedDataService> _logger;

        public AggregatedDataService(
            IOpenWeatherService openWeatherService,
            IGithubApiService githubApiService,
            INewsApiService newsApiService,
            ILogger<AggregatedDataService> logger

        )
        {
            _openWeatherService = openWeatherService;
            _githubApiService = githubApiService;
            _newsApiService = newsApiService;
            _logger = logger;
        }


        public async Task<AggregationDto> GetAggregatedDataAsync(AggregationQuery query)
        {

            AggregationDto aggregationResult = new AggregationDto();

            // Weather
            try
            {
                aggregationResult.OpenWeatherResponse = await _openWeatherService.GetOpenWeatherAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "OpenWeather API failed. Returning empty forecast.");
                aggregationResult.OpenWeatherResponse = new List<OpenWeatherDTO>();
            }

            // GitHub
            try
            {
                aggregationResult.GithubResponse = await _githubApiService.GetGithubUsersByLocationAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "GitHub API failed. Returning empty user list.");
                aggregationResult.GithubResponse = new GithubApiDTO { TotalCount = 0, Users = new List<GithubUserDto>() };
            }

            // News
            try
            {
                aggregationResult.NewsResponse = await _newsApiService.GetNewsAsync(query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "News API failed. Returning empty articles.");
                aggregationResult.NewsResponse = new NewsApiFullDto { TotalResults = 0, Articles = new List<NewsApiDto>() };
            }

            return aggregationResult;
        }
    }
}
