using CowCount.Models;

namespace CowCount.Services.Interfaces
{
    public interface ISavedDataService
    {
        Task<Data> GetDataAsync(CancellationToken token);
        Task UpdateDataAsync(Data config, CancellationToken token);
    }
}