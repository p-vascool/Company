namespace Company.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Web.ViewModels.Destinations.ViewModels;
    using Company.Web.ViewModels.Trips.ViewModels;

    public interface IDestinationsService
    {
        IEnumerable<DestinationViewModel> GetAllDestinationsAsync();

        Task<TripSearchViewModel> GetSearchResultAsync(int fromDestinationId, int toDestinationId, DateTime? dateOfDeparture);
    }
}
