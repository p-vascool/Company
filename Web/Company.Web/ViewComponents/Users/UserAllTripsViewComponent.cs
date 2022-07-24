namespace Company.Web.ViewComponents.Users
{
    using System.Threading.Tasks;

    using Company.Services.Data.Contracts;
    using Company.Web.ViewModels.Trips.ViewModels;
    using Microsoft.AspNetCore.Mvc;

    public class UserAllTripsViewComponent : ViewComponent
    {
        private readonly ITripsService tripsService;

        public UserAllTripsViewComponent(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var allUserTrips = this.tripsService.GetUserTripsWithUsername<TripDetailsViewModel>(username);

            TripListViewModel model = new TripListViewModel
            {
                Trips = allUserTrips,
            };

            return this.View(model);
        }
    }
}
