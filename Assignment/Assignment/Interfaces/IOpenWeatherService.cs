using Assignment.Models;

namespace Assignment.Interfaces
{
    public interface IOpenWeatherService
    {
        Task<List<OpenWeatherDTO>> GetOpenWeatherAsync(AggregationQuery query);
    }

}
