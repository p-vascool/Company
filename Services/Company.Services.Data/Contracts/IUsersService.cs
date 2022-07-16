namespace Company.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using Company.Data.Models;
    using Company.Web.ViewModels.Users.ViewModels;

    public interface IUsersService
    {
        Task<ApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser currentUser);

        T GetByUsername<T>(string username);

        ApplicationUserViewModel GetUserByUsername(string username);

        ApplicationUser GetUserById(string id);

        Task AcceptTripRequest(string senderId, ApplicationUser user, string tripId);

        Task DeclineTripRequest(string senderId, ApplicationUser user, string tripId);

        Task<bool> IsUserExists(string username);

        Task<int> TakeCreatedTripPostsCountByUsername(string username);

        double ExtractUserRatingScore(string username);

        Task<int> GetLatestScore(ApplicationUser user, string username);

        bool IsRequestAlreadySent(string senderId, string tripId);

        Task<double> RateUser(ApplicationUser currentUser, string username, int rate);
    }
}
