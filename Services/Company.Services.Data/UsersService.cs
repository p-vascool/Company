namespace Company.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Data.Models;
    using Company.Data.Models.Enums;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Web.Infrastructure.Contracts;
    using Company.Web.Infrastructure.Hubs;
    using Company.Web.ViewModels.Users.ViewModels;
    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _hubcontext;
        private readonly INotificationService _notificationsService;

        public UsersService(
            IUnitOfWork unitOfWork,
            IHubContext<ChatHub> hubcontext,
            INotificationService notificationsService)
        {
            this._unitOfWork = unitOfWork;
            this._hubcontext = hubcontext;
            this._notificationsService = notificationsService;
        }

        public async Task AcceptTripRequest(string senderId, ApplicationUser user, string tripId)
        {
            ArgumentNullException.ThrowIfNull(user, $"There is no user with Id {user.Id}");

            ApplicationUser passenger = this.GetUserById(senderId);
            ArgumentNullException.ThrowIfNull(passenger, $"There is no passenger with Id {passenger.Id}");

            var trip = await this._unitOfWork
                                    .Trips
                                    .All()
                                    .Where(x => x.Id == tripId)
                                    .Include(x => x.TripRequest)
                                    .Include(x => x.Car)
                                    .Include(x => x.Passengers)
                                    .FirstOrDefaultAsync();

            if (trip.Passengers.Count() >= trip.Car.Seats)
            {
                throw new Exception("There is no more free seats avaliable.");
            }

            var tripRequest = trip.TripRequest.Where(x => x.TripId == tripId && x.UserId == senderId).FirstOrDefault();
            tripRequest.Status = RequestStatus.Accepted;
            trip.Passengers.Add(passenger);

            var notificationId = await this._notificationsService.AddAcceptedTripRequestNotification(user.UserName, passenger.UserName, "You have been acceppted for trip", tripId);
            var notification = await this._notificationsService.GetNotificationByIdAsync(notificationId);
            await this._hubcontext.Clients.User(senderId).SendAsync("VisualizeNotification", notification);

            this._unitOfWork.Trips.Update(trip);
            this._unitOfWork.TripRequests.Update(tripRequest);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task DeclineTripRequest(string senderId, ApplicationUser user, string tripId)
        {
            var trip = await this._unitOfWork
                                    .Trips
                                    .All()
                                    .Where(x => x.Id == tripId)
                                    .Include(x => x.TripRequest)
                                    .Include(x => x.Car)
                                    .Include(x => x.Passengers)
                                    .FirstOrDefaultAsync();

            var passenger = this.GetUserById(senderId);
            var tripRequest = await this._unitOfWork
                                    .TripRequests
                                    .All()
                                    .Where(x => x.TripId == tripId && x.UserId == senderId)
                                    .FirstOrDefaultAsync();
            tripRequest.Status = RequestStatus.Declined;

            var notificationId = await this._notificationsService.AddDeclinedTripRequestNotification(user.UserName, passenger.UserName, "You trip request has been declined for trip", tripId);
            var notification = await this._notificationsService.GetNotificationByIdAsync(notificationId);
            await this._hubcontext.Clients.User(senderId).SendAsync("VisualizeNotification", notification);

            this._unitOfWork.TripRequests.Update(tripRequest);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task<ApplicationUserViewModel> ExtractUserInfo(string username, ApplicationUser currentUser)
        {
            var user = await this._unitOfWork.Users.All().FirstOrDefaultAsync(u => u.UserName == username);
            var group = new List<string>() { username, currentUser.UserName };

            var model = new ApplicationUserViewModel
            {
                Id = user.Id,
                Username = user.UserName,
                RegisteredOn = user.RegisteredOn,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                AboutMe = user.AboutMe,
                ImageUrl = user.ImageUrl,
                CoverImageUrl = user.CoverImageUrl,
                IsBlocked = user.IsBlocked,
                GitHubUrl = user.GitHubUrl,
                FacebookUrl = user.FacebookUrl,
                InstagramUrl = user.InstagramUrl,
                LinkedinUrl = user.LinkedinUrl,
                TwitterUrl = user.TwitterUrl,
                StackoverflowUrl = user.StackoverflowUrl,
                GroupName = string.Join("->", group.OrderBy(x => x)),
            };

            return model;
        }

        public double ExtractUserRatingScore(string username)
        {
            return this.CalculateRatingScore(username);
        }

        public T GetByUsername<T>(string username)
        {
            var user = this._unitOfWork.Users.All().FirstOrDefault(x => x.UserName == username);
            var userT = AutoMapperConfig.MapperInstance.Map<T>(user);

            return userT;
        }

        public async Task<int> GetLatestScore(ApplicationUser user, string username)
        {
            var target = await this._unitOfWork.UserRatings
              .All()
              .FirstOrDefaultAsync(x => x.Username == username && x.RaterUsername == user.UserName);

            return target == null ? 0 : target.Stars;
        }

        public ApplicationUser GetUserById(string id)
        {
            return this._unitOfWork.Users.All().FirstOrDefault(x => x.Id == id);
        }

        public ApplicationUserViewModel GetUserByUsername(string username)
        {
            var user = this._unitOfWork.Users.All().FirstOrDefault(x => x.UserName == username);

            var userT = AutoMapperConfig.MapperInstance.Map<ApplicationUserViewModel>(user);

            return userT;
        }

        public bool IsRequestAlreadySent(string senderId, string tripId)
        {
            return this._unitOfWork.TripRequests.All().Any(x => x.UserId == senderId && x.TripId == tripId);
        }

        public async Task<bool> IsUserExists(string username)
        {
            return await this._unitOfWork.Users.All().AnyAsync(x => x.UserName == username);
        }

        public async Task<double> RateUser(ApplicationUser currentUser, string username, int rate)
        {
            var user = await this._unitOfWork.Users.All().FirstOrDefaultAsync(x => x.UserName == username);

            var targetRating = await this._unitOfWork.UserRatings
                .All()
                .FirstOrDefaultAsync(x => x.Username == username && x.RaterUsername == currentUser.UserName);

            if (targetRating != null)
            {
                targetRating.Stars = rate;
                this._unitOfWork.UserRatings.Update(targetRating);
            }
            else
            {
                targetRating = new UserRating
                {
                    RaterUsername = currentUser.UserName,
                    Username = username,
                    Stars = rate,
                };
                await this._unitOfWork.UserRatings.AddAsync(targetRating);
            }

            await this._unitOfWork.CompleteAsync();

            if (currentUser.UserName != username)
            {
                string notificationId =
                       await this._notificationsService
                       .AddProfileRatingNotification(user, currentUser, rate);

                var count = await this._notificationsService.GetUserNotificationsCount(user.UserName);
                await this._hubcontext
                    .Clients
                    .User(user.Id)
                    .SendAsync("ReceiveNotification", count, true);

                var notificationForApproving = await this._notificationsService.GetNotificationByIdAsync(notificationId);
                await this._hubcontext.Clients.User(user.Id)
                    .SendAsync("VisualizeNotification", notificationForApproving);
            }

            return this.CalculateRatingScore(username);
        }

        public async Task<int> TakeCreatedTripPostsCountByUsername(string username)
        {
            return await this._unitOfWork.UserTrips.All().Where(x => x.User.UserName == username).CountAsync();
        }

        private double CalculateRatingScore(string username)
        {
            double score;
            var count = this._unitOfWork.UserRatings.All().Where(x => x.Username == username).Count();
            if (count != 0)
            {
                var totalScore = this._unitOfWork.UserRatings.All().Where(x => x.Username == username).Sum(x => x.Stars);
                score = Math.Round((double)totalScore / count, 2);

                return score;
            }

            return 0;
        }
    }
}
