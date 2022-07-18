namespace Company.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Company.Common;
    using Company.Data.Models;
    using Company.Services.Data.Contracts;
    using Company.Web.Common;
    using Company.Web.ViewModels.Car.ViewModels;
    using Company.Web.ViewModels.Destinations.ViewModels;
    using Company.Web.ViewModels.Trips.InputModels;
    using Company.Web.ViewModels.Trips.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using X.PagedList;

    public class TripsController : BaseController
    {
        private readonly ITripsService tripsService;
        private readonly ICarsService carsService;
        private readonly IViewService viewService;
        private readonly ITripRequestsService tripRequestsService;
        private readonly IDestinationsService destinationsService;
        private readonly UserManager<ApplicationUser> userManager;

        public TripsController(
            ITripsService tripsService,
            ICarsService carsService,
            IViewService viewService,
            ITripRequestsService tripRequestsService,
            IDestinationsService destinationsService,
            UserManager<ApplicationUser> userManager)
        {
            this.tripsService = tripsService;
            this.carsService = carsService;
            this.viewService = viewService;
            this.tripRequestsService = tripRequestsService;
            this.destinationsService = destinationsService;
            this.userManager = userManager;
        }

        public IActionResult Index(int? page)
        {
            var pageNumber = page ?? 1;
            this.ViewData["Destinations"] = SelectListGenerator.GetAllDestinations(this.destinationsService);
            var trips = this.tripsService.GetTripPosts<TripDetailsViewModel>();

            var viewModel = new TripListViewModel
            {
                Trips = trips.ToPagedList(pageNumber, GlobalConstants.ItemsPerPage),
                SearchQuery = new TripSearchInputModel(),
            };

            return this.View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Search()
        {
            var fromDestination = int.Parse(this.Request.Query["FromDestinationId"]);
            var toDestination = int.Parse(this.Request.Query["ToDestinationId"]);
            var date = DateTime.TryParse(this.Request.Query["DateOfDeparture"], out DateTime dateOfDeparture);

            this.ViewData["Destinations"] = SelectListGenerator.GetAllDestinations(this.destinationsService);

            TripSearchViewModel searchResultViewModel = await this.destinationsService.GetSearchResultAsync(fromDestination, toDestination, dateOfDeparture);
            return this.PartialView("_SearchResultPartial", searchResultViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var cars = this.carsService.GetAllUserCarsByUserId<CarDropDownViewModel>(currentUser.Id);

            var viewModel = new TripCreateInputModel
            {
                ApplicationUser = currentUser,
                Cars = cars,
                Destinations = await this.tripsService.GetAllDestinationsAsync<DestinationViewModel>(),
                DateOfDeparture = DateTime.Now,
            };

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(TripCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);
            input.ApplicationUser = user;
            await this.tripsService.CreateAsync(input);

            return this.RedirectToAction("Index", "Trips");
        }

        [HttpGet]
        [Authorize]
        [Route("Trips/Details/{id}/{sendRequest?}")]
        public async Task<IActionResult> Details(string id, bool? sendRequest)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            await this.viewService.AddViewAsync(id);
            var tripViewModel = this.tripsService.GetById<TripDetailsViewModel>(id);
            var tripRequests = this.tripRequestsService.GetAllTripRequestsByTripId(id);

            var isRequestAlreadySend = await this.tripRequestsService.IsRequestAlreadySend(currentUser.Id, id);
            {
                this.TempData["RequestMessage"] = GlobalConstants.SuccessfullySentTripRequest;
            }

            var sendRequestBoolean = sendRequest.HasValue ? sendRequest.Value : false;

            tripViewModel.TripRequests = tripRequests;
            tripViewModel.SendRequest = sendRequestBoolean;

            if (tripViewModel == null)
            {
                return this.NotFound();
            }

            return this.View(tripViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TripEditInputModel tripToEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(tripToEditViewModel);
            }

            await this.tripsService.EditAsync(tripToEditViewModel);

            return this.RedirectToAction("Details", "Trips", new { area = "", id = tripToEditViewModel.Id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var userId = this.userManager.GetUserId(this.User);
            var destinations = await this.tripsService.GetAllDestinationsAsync<DestinationViewModel>();

            var cars = this.carsService.GetAllUserCarsByUserId<CarDropDownViewModel>(userId);
            var tripToEdit = this.tripsService.GetById<TripEditInputModel>(id);

            tripToEdit.Cars = cars;
            tripToEdit.Destinations = destinations;

            return this.View(tripToEdit);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var tripExist = await this.tripsService.IsTripExist(id, currentUser.UserName);

            if (tripExist)
            {
                await this.tripsService.DeleteAsync(id);
            }
            else
            {
                return this.Unauthorized();
            }

            return this.RedirectToAction("Profile", "Users", new { username = currentUser.UserName, tab = "UserAllTrips" });
        }

        public async Task<IActionResult> Candidate(string tripId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var trip = this.tripsService.GetById(tripId);
            var ownerId = trip.UserId;

            var sendRequest = await this.tripRequestsService.SendTripRequest(currentUser.UserName, trip, ownerId);

            return this.RedirectToAction("Details", new { id = tripId, sendRequest = sendRequest });
        }
    }
}
