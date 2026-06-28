using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimplyToDo.Data.Services;
using SimplyToDo.Data.Services.ToDoTasks;

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

        services.AddIdentity<AppUser, IdentityRole>(options =>
        {
            options.SignIn.RequireConfirmedAccount = true;
        })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders()
            .AddSignInManager();

        // User services
        services.AddScoped<IAccountManager, AccountManager>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserAccessor, HttpContextUserAccessor>();

        // ToDoTaskServices
        services.AddScoped<IUpdateToDoTaskService, IUpdateToDoTaskService>();

        return services;
    }
}
