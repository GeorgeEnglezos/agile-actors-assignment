using Assignment.Configuration;
using Assignment.Interfaces;
using Assignment.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace Assignment.Services
{
    public class GithubApiService : IGithubApiService
    {
        private readonly HttpClient _httpClient;
        private readonly GithubSettings _settings;
        private readonly ILogger<GithubApiService> _logger;

        public GithubApiService(
            HttpClient httpClient,
            IOptions<ApiSettings> apiSettings,
            ILogger<GithubApiService> logger
            )
        {
            _httpClient = httpClient;
            _settings = apiSettings.Value.ExternalApis.Github;
            _logger = logger;

            if (!string.IsNullOrWhiteSpace(_settings.BaseUrl))
            {
                _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
            }
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AgileActorsAssignment", "1.0"));
        }

        public async Task<GithubApiDTO> GetGithubUsersByLocationAsync(AggregationQuery query)
        {
            GithubApiDTO ghResponse = new GithubApiDTO();
            try
            {
                var url = $"search/users?q=location:\"{Uri.EscapeDataString(query.City + " " + query.Country)}\"&per_page={query.PageSize}";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("GitHub API request failed");

                var json = await response.Content.ReadAsStringAsync();

                var apiResponse = JsonSerializer.Deserialize<GithubApiResponse>(json);

                if (apiResponse == null)
                    throw new Exception("Failed to parse GitHub response");


                ghResponse = MapToDto(apiResponse, apiResponse.TotalCount);

                foreach (var user in apiResponse.Items)
                {
                    var detailResp = await _httpClient.GetAsync($"users/{user.Login}");
                    if (!detailResp.IsSuccessStatusCode)
                        continue;
                    // skip if failed

                    var userInfoJson = await detailResp.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<UserInfo>(userInfoJson);

                    if (userInfo != null)
                    {
                        MapUserToDto(ghResponse, userInfo);
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "GitHub service failed");
                return new GithubApiDTO { TotalCount = 0, Users = new List<GithubUserDto>() };
            }
            return ghResponse;
        }


        private static GithubApiDTO MapToDto(GithubApiResponse response, int totalCount)
        {
            GithubApiDTO dtoObject = new GithubApiDTO
            {
                TotalCount = totalCount,                
                Users = new List<GithubUserDto>()
            };
            return dtoObject;
        }

        private static GithubApiDTO MapUserToDto(GithubApiDTO ghResponse, UserInfo userInfo)
        {
            ghResponse.Users.Add(new GithubUserDto
            {
                Company = userInfo.Company ?? string.Empty,
                Username = userInfo.Username,
                AvatarUrl = userInfo.AvatarUrl,
                UserPage = userInfo.UserPage,
                Location = userInfo.Location
            });

            return ghResponse;
        }
    }
}
