namespace Company.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Web.ViewModels.Destinations.ViewModels;
    using Company.Web.ViewModels.Trips.ViewModels;
    using Microsoft.EntityFrameworkCore;

    public class DestinationsService : IDestinationsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DestinationsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<DestinationViewModel> GetAllDestinationsAsync()
        {
            var destinations = this._unitOfWork.Destinations
                 .All()
                 .To<DestinationViewModel>()
                 .ToList();

            return destinations;
        }

        public async Task<TripSearchViewModel> GetSearchResultAsync(int fromDestinationId, int toDestinationId, DateTime? departureTime)
        {
            var fromDestination = await this._unitOfWork.Destinations.All().Where(x => x.Id == fromDestinationId).FirstOrDefaultAsync();
            var toDestination = await this._unitOfWork.Destinations.All().Where(x => x.Id == toDestinationId).FirstOrDefaultAsync();

            var trips = this._unitOfWork.Trips.All()
                .Where(x => x.FromName == fromDestination.Name && x.ToName == toDestination.Name)
                .To<TripDetailsViewModel>()
                .ToList();

            if (departureTime != null)
            {
                trips.Where(x => x.DateOfDeparture == departureTime);
            }

            var serachResultViewModel = new TripSearchViewModel
            {
                Trips = trips,
                DateOfDeparture = departureTime,
            };

            return serachResultViewModel;
        }
    }
}
