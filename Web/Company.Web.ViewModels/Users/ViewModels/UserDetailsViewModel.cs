namespace Company.Web.ViewModels.Users.ViewModels
{
    using Company.Data.Models;
    using Company.Services.Mapping;

    public class UserDetailsViewModel : IMapFrom<ApplicationUser>
    {
        public UserViewModel User { get; set; }
    }
}
