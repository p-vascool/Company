namespace Company.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Common;
    using Company.Data.Models;

    public class CarSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var random = new Random();

            for (int i = 1; i <= 36; i++)
            {
                var user = dbContext.Users.FirstOrDefault(x => x.UserName == $"FooUser{i}");

                var car = new Car
                {
                    User = user,
                    UserId = user.Id,
                    ImageUrl = GlobalConstants.NoCarPictureLocation,
                    Brand = $"FooBrand{i}",
                    Model = $"FooModel{i}",
                    Color = $"FooColor{i}",
                    Manifactured = 2000 + random.Next(0, 20),
                    Seats = random.Next(1, 5),
                    IsAirConditioningAvailable = false,
                    IsSmokingAllowed = true,
                    IsAllowedForPets = false,
                    IsLuggageAvaliable = true,
                };

                user.Cars.Add(car);
                await dbContext.Cars.AddAsync(car);
            }
        }
    }
}
