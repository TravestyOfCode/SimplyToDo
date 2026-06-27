using Microsoft.Extensions.DependencyInjection;

namespace SimplyToDo.Data;

public static class DataSetupService
{
    public static IServiceCollection AddDataServices(this IServiceCollection services, string connectionString)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(connectionString);

        return services;
    }
}
