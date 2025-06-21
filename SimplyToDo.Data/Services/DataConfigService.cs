using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SimplyToDo.Data.Services;

public static class DataConfigService
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrEmpty(connectionString);

        services.AddDbContext<AppDbContext>(o => o.UseSqlServer(connectionString))
            .AddDefaultIdentity<AppUser>(o => o.SignIn.RequireConfirmedAccount = true)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<AppDbContext>();

        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddMediatR(c => c.RegisterServicesFromAssembly(System.Reflection.Assembly.GetExecutingAssembly()));

        return services;
    }
}
