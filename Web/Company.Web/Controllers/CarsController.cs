namespace Company.Web.Controllers
{
    using System.Threading.Tasks;

    using Company.Common;
    using Company.Data.Models;
    using Company.Services;
    using Company.Services.Data.Contracts;
    using Company.Web.ViewModels.Car.InputModels;
    using Company.Web.ViewModels.Car.ViewModels;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class CarsController : BaseController
    {
        private readonly ICarsService carsService;
        private readonly ICloudinaryService cloudinaryService;
        private readonly UserManager<ApplicationUser> userManager;

        public CarsController(
            ICarsService carsService,
            ICloudinaryService cloudinaryService,
            UserManager<ApplicationUser> userManager)
        {
            this.carsService = carsService;
            this.cloudinaryService = cloudinaryService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var userCars = this.carsService.GetAllUserCarsByUserId<CarViewModel>(currentUser.Id);
            var viewModel = new CarListViewModel
            {
                Cars = userCars,
                UserId = currentUser.Id,
                UserUsername = currentUser.UserName,
            };
            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromForm] CarCreateInputModel input)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            await this.carsService.CreateAsync(currentUser.Id, input);
            return this.RedirectToAction("Index", "Cars");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isCarExists = this.carsService.IsCarExists(currentUser.Id, id);
            if (isCarExists)
            {
                CarEditInputModel carEditInput = await this.carsService.ExtractCar(currentUser.Id, id);
                return this.View(carEditInput);
            }

            return this.Unauthorized();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CarEditInputModel inputModel)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View();
            }

            if (inputModel.CarPicture != null)
            {
                string carImageUrl = await this.cloudinaryService.UploadImageAsync(
                                       inputModel.CarPicture,
                                       string.Format(GlobalConstants.CloudinaryCarPictureName, currentUser.Id));
                inputModel.CarImageUrl = carImageUrl;
            }

            await this.carsService.EditAsync(currentUser.Id, inputModel);

            return this.RedirectToAction("Profile", "Users", new { username = currentUser.UserName, tab = "UserAllRegisteredVehicles" });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);
            var isCarExist = this.carsService.IsCarExists(currentUser.Id, id);
            if (!isCarExist)
            {
                return this.Unauthorized();
            }

            await this.carsService.DeleteAsync(currentUser.Id, id);

            return this.RedirectToAction("Profile", "Users", new { username = currentUser.UserName, tab = "UserAllRegisteredVehicles" });
        }
    }
}
