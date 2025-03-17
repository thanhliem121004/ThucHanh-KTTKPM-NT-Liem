using ASC.DataAccess.Interfaces;
using ASC.DataAccess;
using ASC.Web.Configuration;
using ASC.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Thêm using cho IConfiguration
using Microsoft.Extensions.DependencyInjection; // Thêm using cho IServiceCollection
using System; // Thêm using cho InvalidOperationException

namespace ASC.Web.Services
{
    public static class DependencyInjection
    {
        // Config services
        public static IServiceCollection AddCongfig(this IServiceCollection services, IConfiguration config)
        {
            // Add AddDbContext with connectionString to mirage database
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                                     throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            // Add Options and get data from appsettings.json with "AppSettings"
            services.AddOptions(); // IOption
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            return services;
        }

        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
            // Add ApplicationDbContext
            services.AddScoped<DbContext, ApplicationDbContext>();

            // Add IdentityUser IdentityUser
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add services
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IIdentitySeed, IdentitySeed>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add cache, Session
            services.AddSession();
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            // Add RazorPages, MVC
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();

            // AddMyDependencyGroup
            services.AddDistributedMemoryCache();
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();

            return services;
        }
    }
}