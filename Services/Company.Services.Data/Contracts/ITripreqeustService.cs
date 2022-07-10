namespace Company.Services.Data.Contracts
{
    using System.Collections.Generic;

    using Company.Data.Models;

    public interface ITripreqeustService
    {
        TripRequest GetById(string id);

        IEnumerable<TripRequest> GetAllTripRequests();

    }
}
