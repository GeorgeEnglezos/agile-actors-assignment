using System.Text.Json.Serialization;

namespace Assignment.Models
{

    //End-user response
    public class NewsApiFullDto {
        public int TotalResults { get; set; }
        public List<NewsApiDto> Articles { get; set; } = new();
    }

    public class NewsApiDto
    {
        public string Author { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string UrlToImage { get; set; } = string.Empty;
        public DateTime PublishedAt { get; set; }
        public SourceDto Source { get; set; }
    }

    public class SourceDto {
        public string id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;

    }


    //Api Response


    public class NewsApiFullResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("totalResults")]
        public int TotalResults { get; set; }

        [JsonPropertyName("articles")]
        public List<NewsApiResponse> Articles { get; set; } = new();
    }

    public class SourceResponse
    {
        [JsonPropertyName("id")]
        public string id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        public string name { get; set; } = string.Empty;
    }

    public class NewsApiResponse
    {
        [JsonPropertyName("source")]
        public SourceResponse Source { get; set; } = new();

        [JsonPropertyName("author")]
        public string Author { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("urlToImage")]
        public string UrlToImage { get; set; } = string.Empty;

        [JsonPropertyName("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }

}