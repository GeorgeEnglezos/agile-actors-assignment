using Assignment.Models;

namespace Assignment.Interfaces
{
    public interface IAggregatedDataService
    {
        Task<AggregationDto> GetAggregatedDataAsync(AggregationQuery query);
    }
}
