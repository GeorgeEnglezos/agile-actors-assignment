using Assignment.Models;

namespace Assignment.Interfaces
{
    public interface INewsApiService
    {
        Task<NewsApiFullDto> GetNewsAsync(AggregationQuery query);
    }
}
