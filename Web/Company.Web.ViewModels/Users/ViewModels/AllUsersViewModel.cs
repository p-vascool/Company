namespace Company.Web.ViewModels.Users.ViewModels
{
    using Company.Data.Models;
    using Company.Data.Models.Enums;
    using Company.Services.Mapping;

    public class AllUsersViewModel : IMapTo<ApplicationUser>
    {
        public int Page { get; set; }

        public AllUsersTab ActiveTab { get; set; }

        public string Search { get; set; }
    }
}
