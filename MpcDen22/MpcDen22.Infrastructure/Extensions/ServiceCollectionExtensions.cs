using Microsoft.Extensions.DependencyInjection;

namespace MpcDen22.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSingleton<IMachineInstant, MachineInstant>();
    }
}