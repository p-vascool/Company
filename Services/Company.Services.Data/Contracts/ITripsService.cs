namespace Company.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Data.Models;
    using Company.Web.ViewModels.Trips.InputModels;

    public interface ITripsService
    {
        Task CreateAsync(TripCreateInputModel tripCreateInputModel);

        Task EditAsync(TripEditInputModel tripEditInputModel);

        Task DeleteAsync(string id);

        Task<bool> IsTripExist(string id, string userName);

        Trip GetById(string id);

        T GetById<T>(string id);

        int Count();

        IEnumerable<T> GetUserTripsWithUsername<T>(string username);

        IEnumerable<T> GetTripPosts<T>(int? take = null, int skip = 0);

        Task<IEnumerable<T>> GetAllDestinationsAsync<T>();
    }
}
