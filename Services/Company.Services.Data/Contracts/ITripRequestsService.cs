namespace Company.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Company.Data.Models;

    public interface ITripRequestsService
    {
        TripRequest GetById(string id);

        IEnumerable<TripRequest> GetAllTripRequestsByTripId(string tripId);

        IEnumerable<TripRequest> GetPendingTripRequestsByTripId(string tripId);

        Task<bool> SendTripRequest(string userName, Trip trip, string ownerId);

        Task<bool> IsRequestAlreadySend(string senderId, string tripId);
    }
}
