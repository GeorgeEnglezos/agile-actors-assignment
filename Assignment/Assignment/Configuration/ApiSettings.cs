namespace Assignment.Configuration
{
    public class ApiSettings
    {
        public ExternalApis ExternalApis { get; set; }
    }

    public class ExternalApis
    {
        public OpenWeatherSettings OpenWeather { get; set; }
        public NewsApiSettings NewsApi { get; set; }
        public GithubSettings Github { get; set; }
    }

    public class OpenWeatherSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class NewsApiSettings
    {
        public string BaseUrl { get; set; }
        public string ApiKey { get; set; }
    }

    public class GithubSettings
    {
        public string BaseUrl { get; set; }
        public string? ApiKey { get; set; }
    }
}
