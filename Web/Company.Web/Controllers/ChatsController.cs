namespace Company.Web.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Data.Models;
    using Company.Web.Infrastructure.Contracts;
    using Company.Web.ViewModels.Chats;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Authorize]
    public class ChatsController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IChatService chatService;
        private readonly IUnitOfWork unitOfWork;

        public ChatsController(
            UserManager<ApplicationUser> userManager,
            IChatService chatService,
            IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.chatService = chatService;
            this.unitOfWork = unitOfWork;
        }

        [Route("Chat/With/{username?}/Group/{group?}")]
        public async Task<IActionResult> Index(string username, string group)
        {
            var currentUser = this.userManager.GetUserAsync(this.User);

            var model = new ChatViewModel
            {
                FromUser = await this.userManager.GetUserAsync(this.User),
                ToUser = this.unitOfWork.Users.All().FirstOrDefault(x => x.UserName == username),
                ChatMessages = await this.chatService.ExtractAllMessages(group),
                GroupName = group,
            };

            return this.View(model);
        }

        [HttpGet]
        [Route("Chat/GroupChat/{group}")]
        public async Task<IActionResult> GroupChat(string group, string tripId)
        {
            var currentUser = await this.userManager.GetUserAsync(this.User);

            var trip = this.unitOfWork.Trips.All().Where(x => x.Id == tripId).Include(x => x.User).Include(x => x.Passengers).FirstOrDefault();

            var model = new GroupChatVIewModel
            {
                Users = trip.Passengers,
                ChatMessages = await this.chatService.ExtractAllMessages(group),
                GroupName = group,
            };

            return this.View(model);
        }
    }
}
