namespace Company.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data;
    using Company.Data.Models;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Web.ViewModels.Trips.InputModels;
    using Microsoft.EntityFrameworkCore;

    public class TripsService : ITripsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TripsService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public int Count()
        {
            return this._unitOfWork.Trips.All().Count();
        }

        public async Task CreateAsync(TripCreateInputModel input)
        {

            var trip = new Trip
            {
                FromName = input.FromDestinationName,
                ToName = input.ToDestinationName,
                User = input.ApplicationUser,
                UserId = input.UserId,
                Car = input.Car,
                CarId = input.CarId,
                PricePerPassenger = input.PricePerPassenger,
                Departure = input.DateOfDeparture,
                TimeOfDeparture = input.TimeOfDeparture,
                AdditionalInformation = input.AdditionalInformation,
                GroupName = Guid.NewGuid().ToString(),
            };

            var userTrip = new UserTrip
            {
                User = trip.User,
                UserId = trip.UserId,
                Trip = trip,
                TripId = trip.Id,
            };

            var user = input.ApplicationUser;
            user.UserTrips.Add(userTrip);
            this._unitOfWork.Users.Update(user);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var tripToDelete = this._unitOfWork.Trips.All().FirstOrDefault(t => t.Id == id);

            if (tripToDelete == null)
            {
                throw new NullReferenceException($"Activity with id {id} not found.");
            }

            tripToDelete.IsDeleted = true;
            tripToDelete.DeletedOn = DateTime.Now;
            this._unitOfWork.Trips.Update(tripToDelete);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task EditAsync(TripEditInputModel tripToEdit)
        {
            var trip = this._unitOfWork.Trips.All().FirstOrDefault(t => t.Id == tripToEdit.Id);

            if (trip == null)
            {
                throw new NullReferenceException($"Activity with id {tripToEdit.Id} not found");
            }

            trip.FromName = tripToEdit.FromDestinationName;
            trip.ToName = tripToEdit.ToDestinationName;
            trip.Departure = tripToEdit.DateOfDeparture;
            trip.AdditionalInformation = tripToEdit.AdditionalInformation;
            trip.CarId = tripToEdit.CarId;

            this._unitOfWork.Trips.Update(trip);
            await this._unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<T>> GetAllDestinationsAsync<T>()
        {
            IQueryable<Destination> query = this._unitOfWork.Destinations.All();

            return await query.To<T>().ToListAsync();
        }

        public Trip GetById(string id)
        {
            var trip = this._unitOfWork.Trips
              .All()
              .Include(x => x.Car)
              .Include(x => x.Passengers)
              .Include(x => x.TripRequest)
              .Include(x => x.Views)
              .Where(x => x.Id == id).FirstOrDefault();

            return trip;
        }

        public T GetById<T>(string id)
        {
            var trip = this._unitOfWork.Trips
               .All()
               .Include(x => x.TripRequest)
               .Include(x => x.Car)
               .Include(x => x.Views)
               .Include(x => x.Passengers)
               .Where(x => x.Id == id)
               .To<T>()
               .FirstOrDefault();

            return trip;
        }

        public IEnumerable<T> GetTripPosts<T>(int? take = null, int skip = 0)
        {
            IEnumerable<T> tripPosts = this._unitOfWork.Trips
                 .AllAsNoTracking()
                 .Include(x => x.Car)
                 .Include(x => x.User)
                 .Include(x => x.UserTrips)
                 .Where(x => x.Car.IsDeleted == false & x.IsDeleted == false)
                 .OrderByDescending(x => x.CreatedOn)
                 .Skip(skip)
                 .To<T>();

            if (take.HasValue)
            {
                tripPosts = tripPosts.Take(take.Value).ToList();
            }

            return tripPosts;
        }

        public IEnumerable<T> GetUserTripsWithUsername<T>(string username)
        {
            var cars = this._unitOfWork.Trips
              .All()
              .Include(x => x.Passengers)
              .Include(x => x.Car)
              .Include(x => x.UserTrips)
              .Where(x => x.User.UserName == username)
              .To<T>()
              .ToList();

            return cars;
        }

        public async Task<bool> IsTripExist(string id, string userName)
        {
            var trip = await this._unitOfWork.Trips.All().AnyAsync(x => x.Id == id && x.User.UserName == userName);

            return trip;
        }
    }
}
