namespace Company.Web.Infrastructure.Hubs
{
    using System.Threading.Tasks;

    using Company.Data.Common.Repositories;
    using Company.Data.Models;
    using Company.Data.Models.Enums;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class NotificationHub : Hub
    {
        private readonly IRepository<UserNotification> userNotificationsRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationHub(
                        IRepository<UserNotification> repository,
                        UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.userNotificationsRepository = repository;
        }

        public async Task GetUserNotificationCount()
        {
            var username = this.Context.User.Identity.Name;
            var targetUser = await this.userManager.GetUserAsync(this.Context.User);

            var notificationCount = await this.userNotificationsRepository
                                                .All()
                                                .CountAsync(x => x.TargetUsername == username && x.Status == NotificationStatus.Unread);

            await this.Clients.User(targetUser.Id).SendAsync("ReceiveNotification", notificationCount);
        }
    }
}
