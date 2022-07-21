namespace Company.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Services.Data.Contracts;
    using Company.Web.ViewModels.WatchListTrips;
    using Microsoft.AspNetCore.Mvc;

    public class WatchListsController : BaseController
    {
        private readonly IWatchListsTripService watchListsService;

        public WatchListsController(IWatchListsTripService watchListsService)
        {
            this.watchListsService = watchListsService;
        }

        public async Task<IActionResult> All()
        {
            IEnumerable<WatchListTripsViewModel> watchListTrips = this.watchListsService.All(this.User.Identity.Name);

            return this.View(watchListTrips);
        }

        public async Task<IActionResult> Add(string tripId)
        {
            await this.watchListsService.AddAsync(tripId, this.User.Identity.Name);

            return this.RedirectToAction(nameof(this.All));
        }

        public async Task<IActionResult> Delete(string tripId)
        {
            await this.watchListsService.DeleteAsync(tripId, this.User.Identity.Name);

            return this.RedirectToAction(nameof(this.All));
        }
    }
}
