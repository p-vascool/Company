namespace Company.Services.Data
{
    using System.Threading.Tasks;

    using Company.Data.Common.Repositories;
    using Company.Data.Models;
    using Company.Services.Data.Contracts;

    public class ViewsService : IViewService
    {
        private readonly ITripsService _tripsService;
        private readonly IRepository<View> _viewRepository;

        public ViewsService(
            ITripsService tripsRequestsService,
            IRepository<View> viewRepository)
        {
            this._tripsService = tripsRequestsService;
            this._viewRepository = viewRepository;
        }

        public async Task AddViewAsync(string tripId)
        {
            var trip = this._tripsService.GetById(tripId);

            var view = new View { Trip = trip, UserId = trip.UserId };
            trip.Views.Add(view);

            await this._viewRepository.AddAsync(view);
            await this._viewRepository.SaveChangesAsync();
        }
    }
}
