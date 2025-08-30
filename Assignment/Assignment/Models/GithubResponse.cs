using System.Text.Json.Serialization;

namespace Assignment.Models
{

    //End-user response
    public class GithubApiDTO
    {
        public int TotalCount { get; set; }
        public List<GithubUserDto> Users { get; set; }
    }

    public class GithubUserDto 
    {
        public string Username { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string AvatarUrl { get; set; } = string.Empty;
        public string UserPage { get; set; } = string.Empty; //HtmlUrl
        public string Location { get; set; } = string.Empty;
    }

    public class GithubApiResponse
    {
        [JsonPropertyName("total_count")]
        public int TotalCount { get; set; }

        [JsonPropertyName("items")]
        public List<GitHubUser> Items { get; set; } = new();
    }

    public class GitHubUser
    {
        [JsonPropertyName("login")]
        public string Login { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;

        [JsonPropertyName("html_url")]
        public string HtmlUrl { get; set; } = string.Empty;
        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;
    }

    public class UserInfo {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("login")]
        public string Username { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public string Location { get; set; } = string.Empty;

        [JsonPropertyName("avatar_url")]
        public string AvatarUrl { get; set; } = string.Empty;
        [JsonPropertyName("html_url")]
        public string UserPage { get; set; } = string.Empty;

        [JsonPropertyName("bio")]
        public string Bio { get; set; } = string.Empty;

        [JsonPropertyName("company")]
        public string Company { get; set; } = string.Empty;
    }
}