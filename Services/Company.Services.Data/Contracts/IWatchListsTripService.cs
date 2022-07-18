namespace Company.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Web.ViewModels.WatchListTrips;

    public interface IWatchListsTripService
    {
        Task AddAsync(string id, string name);

        IEnumerable<WatchListTripsViewModel> All(string name);

        Task DeleteAsync(string id, string userName);
    }
}
