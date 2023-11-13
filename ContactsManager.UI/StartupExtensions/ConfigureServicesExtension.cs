using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.Domain.RepositoryContracts;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DBContext;
using ContactsManager.Infrastructure.Repositories;
using ContactsManager.UI.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace ContactsManager.UI.StartupExtensions { 
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews(options =>
            {
                var logger = services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
                //options.Filters.Add<ResponseHeaderActionFilter>();
                options.Filters.Add(new ResponseHeaderActionFilter("My-Key-From-Global", "My-Value-From-Global", 2));
            });

            services.AddScoped<ICountryService, CountryService>();

            services.AddScoped<IPersonGetterService, PersonGetterService>();
            services.AddScoped<IPersonAddService, PersonAddService>();
            services.AddScoped<IPersonDeleteService, PersonDeleteService>();
            services.AddScoped<IPersonUpdateService, PersonUpdateService>();
            services.AddScoped<IPersonSortService, PersonSortService>();

            services.AddScoped<IPeopleRepository, PeopleRepository>();
            services.AddScoped<ICountriesRepository, CountriesRepository>();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration["ConnectionStrings:DefaultConnection"]);
      
            });

            //Enable Identity in this Project
            services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredUniqueChars = 3; })
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, AppDbContext, Guid>>()
                    .AddRoleStore<RoleStore<ApplicationRole, AppDbContext, Guid>>();

            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build(); 
            });

            services.ConfigureApplicationCookie(options => {
                options.LoginPath = "/Account/Login";
            });

        }
    }
}
