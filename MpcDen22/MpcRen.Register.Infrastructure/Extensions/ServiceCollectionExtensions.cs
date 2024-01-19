using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MpcRen.Register.Infrastructure.Engine;
using MpcRen.Register.Infrastructure.Net;
using MpcRen.Register.Infrastructure.Sharing;

namespace MpcRen.Register.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddSecretSharingServices();
    }

    public static void AddSecretSharingServices(this IServiceCollection services)
    {
        services.AddSingleton<ISecretShareService, SecretShareService>();
    }
    public static void AddNetworkServices(this IServiceCollection services)
    {
        services.AddSingleton<INetworkService, NetworkService>();
    }
    public static void AddCommandFactory(this IServiceCollection services)
    {
        services.AddSingleton<ICommandFactory, CommandFactory>();
    }
}