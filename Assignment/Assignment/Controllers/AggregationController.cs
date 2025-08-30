using Assignment.Interfaces;
using Assignment.Models;
using Assignment.Services;
using Microsoft.AspNetCore.Mvc;

namespace Assignment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AggregationController : ControllerBase
    {
        private readonly ILogger<AggregationController> _logger;
        private readonly IAggregatedDataService _aggregatedDataService;

        public AggregationController(
            ILogger<AggregationController> logger,
            IAggregatedDataService aggregatedDataServic
        )
        {
            _logger = logger;
            _aggregatedDataService = aggregatedDataServic;
        }


        /// <summary>
        /// Retrieves aggregated data for a specific location including weather, GitHub users, and news
        /// </summary>
        /// <param name="query">The aggregation query containing city and country information</param>
        /// <returns>Aggregated data containing weather information, GitHub users by location, and relevant news</returns>
        /// <remarks>
        /// This endpoint aggregates data from multiple external APIs:
        /// - OpenWeather API for weather information
        /// - GitHub API for users in the specified location  
        /// - News API for location-relevant news
        /// 
        /// Sample request:
        /// 
        ///     POST /api/aggregation/location
        ///     Content-Type: application/json
        ///     {
        ///         "city": "Athens",
        ///         "country": "GR",
        ///         "pageSize": 5,
        ///         "newsKeyword": "Tech",
        ///         "newsSortBy": "publishedAt",
        ///         "fromDate": "2025-08-02T00:00:00Z",
        ///         "toDate": "2025-09-02T23:59:59Z",
        ///         "minTemperature": 0,
        ///         "maxTemperature": 100
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">Returns the aggregated location data successfully</response>
        /// <response code="400">If city or country parameters are missing or invalid</response>
        /// <response code="500">If there's an internal server error during API calls</response>
        [HttpPost("location")]
        public async Task<IActionResult> GetLocation([FromBody] AggregationQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.City) || string.IsNullOrWhiteSpace(query.Country))
            {
                return BadRequest("City and country parameters are required.");
            }

            _logger.LogInformation("Calling apis for {City}, {Country}", query.City, query.Country);

            AggregationDto response = await _aggregatedDataService.GetAggregatedDataAsync(query);
            return Ok(response);
        }


        /// <summary>
        /// Health check endpoint to verify API availability and status
        /// </summary>
        /// <returns>Current health status of the API with timestamp</returns>
        /// <remarks>
        /// Simple health check endpoint that returns the current status and timestamp.
        /// 
        /// Sample response:
        /// 
        ///     {
        ///        "status": "Healthy",
        ///        "timestamp": "2024-01-15T10:30:00.000Z"
        ///     }
        /// 
        /// </remarks>
        /// <response code="200">API is healthy and operational</response>
        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Timestamp = DateTime.UtcNow });
        }
    }
}
