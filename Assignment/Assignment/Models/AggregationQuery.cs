using System.Text.Json.Serialization;

namespace Assignment.Models
{
    public class AggregationQuery
    {
        // Shared filters
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public int PageSize { get; set; } = 30; // Results number
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string NewsKeyword { get; set; } = "Tech";
        public string NewsSortBy { get; set; } = "publishedAt";

        public string WeatherCondition { get; set; } = string.Empty;
        public double? MinTemperature { get; set; }
        public double? MaxTemperature { get; set; }
    }
}