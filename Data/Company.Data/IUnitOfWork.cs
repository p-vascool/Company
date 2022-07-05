namespace Company.Data
{
    using System.Threading.Tasks;

    using Company.Data.Common.Repositories;
    using Company.Data.Models;

    public interface IUnitOfWork
    {
        IDeletableEntityRepository<Car> Cars { get; }

        IDeletableEntityRepository<ApplicationUser> Users { get; }

        IDeletableEntityRepository<Group> Groups { get; }

        IDeletableEntityRepository<ChatMessage> ChatMessages { get; }

        IDeletableEntityRepository<Country> Countries { get; }

        IDeletableEntityRepository<CountryCode> CountryCodes { get; }

        IDeletableEntityRepository<State> States { get; }

        IDeletableEntityRepository<City> Cities { get; }

        IDeletableEntityRepository<ZipCode> ZipCodes { get; }

        IRepository<Destination> Destinations { get; }

        IRepository<Trip> Trips { get; }

        IRepository<TripRequest> TripRequests { get; }

        IRepository<UserTrip> UserTrips { get; }

        IRepository<UserRating> UserRatings { get; }

        Task CompleteAsync();
    }
}
