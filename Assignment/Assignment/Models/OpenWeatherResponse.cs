using System.Text.Json.Serialization;

namespace Assignment.Models
{
    //End-user response
    public class OpenWeatherDTO
    {
        public string City { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public double WindSpeed { get; set; }
        public DateTime Timestamp { get; set; }
        public string Condition { get; set; } = string.Empty;
    }

    public class OpenWeatherResponse
    {
        [JsonPropertyName("city")]
        public ForecastCity City { get; set; } = new();

        [JsonPropertyName("list")]
        public List<ForecastItem> List { get; set; } = new();
    }

    public class ForecastCity
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;
    }

    public class ForecastItem
    {
        [JsonPropertyName("dt_txt")]
        public string DtTxt { get; set; }

        [JsonPropertyName("main")]
        public Main Main { get; set; } = new();

        [JsonPropertyName("weather")]
        public Weather[] Weather { get; set; } = Array.Empty<Weather>();

        [JsonPropertyName("wind")]
        public Wind Wind { get; set; } = new();
    }

    public class Main
    {
        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double FeelsLike { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("main")]
        public string Main { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("icon")]
        public string Icon { get; set; } = string.Empty;
    }

    public class Wind
    {
        [JsonPropertyName("speed")]
        public double Speed { get; set; }
    }
}
