namespace Company.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Web.ViewModels.Trips.ViewModels;

    public interface IDestinationsService
    {
        IEnumerable<TripSearchViewModel> GetAllDestinations();

        Task<TripSearchViewModel> GetSearchResultAsync(int fromDestinationId, int toDestinationId, DateTime? departureTime);
    }
}
