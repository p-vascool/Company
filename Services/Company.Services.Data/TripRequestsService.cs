namespace Company.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Data.Models;
    using Company.Data.Models.Enums;
    using Company.Services.Data.Contracts;
    using Company.Web.Infrastructure.Hubs;
    using Company.Web.Infrastructure.Contracts;

    using Microsoft.AspNetCore.SignalR;
    using Microsoft.EntityFrameworkCore;

    public class TripRequestsService : ITripRequestsService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService notificationService;

        public TripRequestsService(
                        IUnitOfWork unitOfWork,
                        IHubContext<NotificationHub> hubContext,
                        INotificationService notificationService)
        {
            this._unitOfWork = unitOfWork;
            this._hubContext = hubContext;
            this.notificationService = notificationService;
        }

        public IEnumerable<TripRequest> GetAllTripRequestsByTripId(string tripId)
        {
            var trips = this._unitOfWork
                               .TripRequests
                               .All()
                               .Include(x => x.User)
                               .Where(x => x.TripId == tripId);

            return trips;
        }

        public TripRequest GetById(string id)
        {
            return this._unitOfWork
                        .TripRequests
                        .All()
                        .Where(x => x.Id == id)
                        .FirstOrDefault();
        }

        public IEnumerable<TripRequest> GetPendingTripRequestsByTripId(string tripId)
        {
            return this._unitOfWork
                        .TripRequests
                        .All()
                        .Include(x => x.User)
                        .Include(x => x.Trip)
                        .Where(x => x.TripId == tripId && x.Status == RequestStatus.Pending);
        }

        public async Task<bool> IsRequestAlreadySend(string senderId, string tripId)
        {
            return await this._unitOfWork.TripRequests
                                            .All()
                                            .AnyAsync(x => x.UserId == senderId && x.TripId == tripId);
        }

        public async Task<bool> SendTripRequest(string usernName, Trip trip, string ownerId)
        {
            var isRequestSent = false;

            var owner = await this._unitOfWork.Users
                                        .All()
                                        .Where(x => x.Id == ownerId)
                                        .Include(x => x.UserTrips)
                                        .Include(x => x.TripRequests)
                                        .FirstOrDefaultAsync();

            var sender = await this._unitOfWork.Users
                                        .All()
                                        .Where(x => x.UserName == usernName)
                                        .FirstOrDefaultAsync();

            if (!await this.IsRequestAlreadySend(sender.Id, trip.Id))
            {
                isRequestSent = true;
                var tripRequest = new TripRequest
                {
                    TripId = trip.Id,
                    Status = RequestStatus.Pending,
                    Trip = trip,
                    User = sender,
                    UserId = sender.Id,
                };
                trip.TripRequest.Add(tripRequest);

                await this._unitOfWork.TripRequests.AddAsync(tripRequest);
                await this._unitOfWork.CompleteAsync();

                var notificationId = await this.notificationService.AddTripRequestNotification(sender.UserName, owner.UserName, $"{sender.UserName} sent a trip request from your trip!", trip.Id);
                var notification = await this.notificationService.GetNotificationByIdAsync(notificationId);
                await this._hubContext.Clients.User(ownerId).SendAsync("VisualizeNotification", notification);
            }

            return isRequestSent;
        }
    }
}
