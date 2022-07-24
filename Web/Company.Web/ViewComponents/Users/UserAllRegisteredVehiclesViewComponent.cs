namespace Company.Web.ViewComponents.Users
{
    using System.Threading.Tasks;

    using Company.Data.Models;
    using Company.Services.Data.Contracts;
    using Company.Web.ViewModels.Car.ViewModels;
    using Company.Web.ViewModels.Users.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class UserAllRegisteredVehiclesViewComponent : ViewComponent
    {
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserAllRegisteredVehiclesViewComponent(
            ICarsService carsService,
            UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            this.carsService = carsService;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string username, int page)
        {
            var allUserCars = this.carsService.GetAllUserCarsByUserName<CarViewModel>(username);
            var user = this.usersService.GetByUsername<ApplicationUserViewModel>(username);

            CarListViewModel model = new CarListViewModel
            {
                UserId = user.Id,
                UserUsername = username,
                Cars = allUserCars,
            };

            return this.View(model);
        }
    }
}
