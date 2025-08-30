using Assignment.Models;

namespace Assignment.Interfaces
{
    public interface IGithubApiService
    {
        Task<GithubApiDTO> GetGithubUsersByLocationAsync(AggregationQuery query);
    }

}
