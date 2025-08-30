using System.Text.Json.Serialization;

namespace Assignment.Models
{

    //End-user response
    public class AggregationDto
    {
        public GithubApiDTO GithubResponse { get; set; }
        public List<OpenWeatherDTO> OpenWeatherResponse { get; set; }
        public NewsApiFullDto NewsResponse { get; set; }
    }
}