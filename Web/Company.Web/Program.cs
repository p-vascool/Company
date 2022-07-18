namespace Company.Web
{
    using System.Linq;
    using System.Reflection;

    using CloudinaryDotNet;
    using Company.Data;
    using Company.Data.Common;
    using Company.Data.Common.Repositories;
    using Company.Data.Models;
    using Company.Data.Repositories;
    using Company.Data.Seeding;
    using Company.Services;
    using Company.Services.Data;
    using Company.Services.Data.Contracts;
    using Company.Services.Mapping;
    using Company.Services.Messaging;
    using Company.Services.Messaging.SecurityModels;
    using Company.Web.Infrastructure;
    using Company.Web.Infrastructure.Contracts;
    using Company.Web.Infrastructure.Hubs;
    using Company.Web.ViewModels;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Twilio;

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureServices(builder.Services, builder.Configuration);
            var app = builder.Build();
            Configure(app);
            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
                .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
               .AddCookie()
               .AddJwtBearer();

            services.Configure<CookiePolicyOptions>(
                options =>
                {
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                });

            services.AddAntiforgery(options =>
            {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            services.AddAuthentication()
              .AddFacebook(facebookOptions =>
              {
                  facebookOptions.AppId = configuration["Facebook:AppId"];
                  facebookOptions.AppSecret = configuration["Facebook:AppSecret"];
              })
              .AddGoogle(googleOptions =>
              {
                  googleOptions.ClientId = configuration["Google:ClientId"];
                  googleOptions.ClientSecret = configuration["Google:ClientSecret"];
              });

            var cloudinaryAccount = new CloudinaryDotNet.Account(
              configuration["Cloudinary:Account"],
              configuration["Cloudinary:ApiKey"],
              configuration["Cloudinary:ApiSecret"]);
            var cloudinary = new Cloudinary(cloudinaryAccount);
            services.AddSingleton(cloudinary);

            services.AddControllersWithViews(
                options =>
                {
                    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                }).AddRazorRuntimeCompilation();
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddSingleton(configuration);

            // Twilio Authentication
            var accountSid = configuration["Twilio:AccountSID"];
            var authToken = configuration["Twilio:AuthToken"];
            TwilioClient.Init(accountSid, authToken);
            services.Configure<TwilioVerifySettings>(configuration.GetSection("Twilio"));
            var test = configuration.GetSection("Twilio:VerificationServiceSID");

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IDbQueryRunner, DbQueryRunner>();

            // Application services
            services.AddTransient<IEmailSender, NullMessageSender>();
            services.AddTransient<IEmailSender>(provider => new SendGridEmailSender(configuration["SendGridApiKey"]));
            services.AddTransient<IDestinationsService, DestinationsService>();
            services.AddTransient<IViewService, ViewsService>();
            services.AddTransient<ITripsService, TripsService>();
            services.AddTransient<ICarsService, CarsService>();
            services.AddTransient<IUsersService, UsersService>();
            services.AddTransient<ITripRequestsService, TripRequestsService>();
            services.AddTransient<IWatchListsTripService, WatchListsTripService>();
            services.AddTransient<ICloudinaryService, CloudinaryService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<INotificationService, NotificationService>();

            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSingleton(configuration);

            services.AddAutoMapper(typeof(Program));
            services.AddControllersWithViews();
            services.AddSignalR();
        }

        private static void Configure(WebApplication app)
        {
            // Seed data on application startup
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                if (!dbContext.Users.Any())
                {
                    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
                }
            }

            AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStatusCodePagesWithRedirects("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(
                endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "areas",
                        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapHub<ChatHub>("/chatHub");
                    endpoints.MapHub<NotificationHub>("/notificationHub");
                    endpoints.MapHub<UserStatusHub>("/userStatusHub");

                    endpoints.MapRazorPages();
                });
        }
    }
}
