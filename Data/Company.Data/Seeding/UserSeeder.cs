﻿namespace Company.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Company.Common;
    using Company.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public class UserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider
                .GetRequiredService<UserManager<ApplicationUser>>();

            for (int i = 1; i <= 36; i++)
            {
                var user = new ApplicationUser
                {
                    UserName = $"FooUser{i}",
                    Email = $"FooBar{i}@besl.bg",
                    ImageUrl = GlobalConstants.NoProfilePictureLocation,
                };

                var result = await userManager.CreateAsync(user, "awedawe1");

                if (i % 3 == 0)
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName);
                }
                else
                {
                    await userManager.AddToRoleAsync(user, GlobalConstants.UserDefaultRoleName);
                }

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                var createdUser = await userManager.FindByNameAsync($"FooPlayer{i}");
            }
        }
    }
}
