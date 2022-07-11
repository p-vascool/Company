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
        private IUnitOfWork _unitOfWork;

        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly INotificationService notificationService;

        public TripRequestsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
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

        public Task<bool> IsRequestAlreadySend(string senderId, string tripId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> SendTripRequest(string senderId, Trip trip, string ownerId)
        {
            throw new System.NotImplementedException();
        }
    }
}
