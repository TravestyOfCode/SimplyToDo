using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplyToDo.Data.Services;

namespace SimplyToDo.Data;

public static class DataSetupService
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddDefaultIdentity<AppUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders(); // Not sure if this is needed or included with AddDefaultIdentity

        services.AddScoped<IAccountManager, AccountManager>();

        return services;
    }
}
