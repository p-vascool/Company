﻿namespace Company.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Company.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class TripSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            var random = new Random();
            var random2 = new Random();
            var dateRandom = new Random();
            var trips = new List<Trip>();

            for (int i = 1; i <= 36; i++)
            {
                var user = dbContext.Users.FirstOrDefault(x => x.UserName == $"FooUser{i}");
                var car = dbContext.Cars.FirstOrDefault(x => x.UserId == user.Id);
                var randomFromId = random.Next(1, 257);
                var randomToId = random2.Next(1, 257);
                var destinationFrom = dbContext.Destinations.Where(x => x.Id == randomFromId).FirstOrDefault();
                var destinationTo = dbContext.Destinations.Where(x => x.Id == randomToId).FirstOrDefault();
                var dateOfDeparture = new DateTime(2020, 12, dateRandom.Next(1, 31));

                var trip = new Trip
                {
                    From = destinationFrom,
                    To = destinationTo,
                    FromName = destinationFrom.Name,
                    ToName = destinationTo.Name,
                    Departure = dateOfDeparture,
                    PricePerPassenger = i + 10,
                    Car = car,
                    User = user,
                    GroupName = Guid.NewGuid().ToString(),
                };
                trips.Add(trip);

                var userTrip = new UserTrip
                {
                    TripId = trip.Id,
                    Trip = trip,
                    UserId = user.Id,
                    User = user,
                };

                user.UserTrips.Add(userTrip);
            }

            await dbContext.Trips.AddRangeAsync(trips);
            await dbContext.SaveChangesAsync();
        }
    }
}
