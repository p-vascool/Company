namespace Company.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Company.Data.Models;
    using Newtonsoft.Json;

    public class DestinationSeeder : ISeeder
    {

        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Trips.Any())
            {
                return;
            }

            var directory = Directory.GetCurrentDirectory();
            var json = File.ReadAllText(directory);
            var destinations = JsonConvert.DeserializeObject<IEnumerable<Destination>>(json);

            await dbContext.Destinations.AddRangeAsync(destinations);
        }
    }
}
