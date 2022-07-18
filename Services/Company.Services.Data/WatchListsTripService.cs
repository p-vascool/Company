namespace Company.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data.Common.Repositories;
    using Company.Data.Models;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Web.ViewModels.WatchListTrips;
    using Microsoft.EntityFrameworkCore;

    public class WatchListsTripService : IWatchListsTripService
    {
        private readonly IDeletableEntityRepository<WatchListTrip> watchListRepository;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly ITripsService tripsService;

        public WatchListsTripService(
            IDeletableEntityRepository<WatchListTrip> watchListRepository,
            IRepository<ApplicationUser> usersRepository,
            ITripsService tripsService)
        {
            this.watchListRepository = watchListRepository;
            this.usersRepository = usersRepository;
            this.tripsService = tripsService;
        }

        public async Task AddAsync(string id, string name)
        {
            var user = this.usersRepository.All().Include(x => x.WatchListTrips).FirstOrDefault(x => x.UserName == name);
            if (user == null || user.WatchListTrips.Any(x => x.TripId == id))
            {
                return;
            }

            var trip = this.tripsService.GetById(id);
            if (trip == null)
            {
                return;
            }

            if (this.watchListRepository.All().Any(x => x.UserId == user.Id && x.TripId == trip.Id))
            {
                return;
            }

            var watchListTrip = new WatchListTrip
            {
                UserId = user.Id,
                TripId = trip.Id,
            };

            user.WatchListTrips.Add(watchListTrip);
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            return;
        }

        public IEnumerable<WatchListTripsViewModel> All(string name)
        {
            var watchListTrips = this.watchListRepository.All().Include(x => x.Trip).ThenInclude(x => x.User)
                                                       .Where(x => x.User.UserName == name && x.IsDeleted == false).To<WatchListTripsViewModel>().ToList();

            if (watchListTrips == null)
            {
                return new List<WatchListTripsViewModel>();
            }

            return watchListTrips;
        }

        public async Task DeleteAsync(string id, string userName)
        {
            var watchListTrip = this.watchListRepository.All().FirstOrDefault(x => x.User.UserName == userName && x.Trip.Id == id);

            if (watchListTrip == null)
            {
                return;
            }

            watchListTrip.IsDeleted = true;
            watchListTrip.DeletedOn = DateTime.Now;

            this.watchListRepository.Update(watchListTrip);
            await this.watchListRepository.SaveChangesAsync();
        }
    }
}
