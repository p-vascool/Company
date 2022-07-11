namespace Company.Web.ViewModels.Users.ViewModels
{
    using System.Collections.Generic;

    using Company.Data.Models;
    using Company.Services.Mapping;
    using Company.Web.ViewModels.Trips.ViewModels;

    public class UserTripListViewModel : IMapFrom<ApplicationUser>
    {
        public IEnumerable<TripDetailsViewModel> Trips { get; set; }
    }
}
