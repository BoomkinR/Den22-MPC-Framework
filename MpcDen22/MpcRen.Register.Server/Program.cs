// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MpcRen.Register.Infrastructure;
using MpcRen.Register.Infrastructure.Extensions;
using MpcRen.Register.Server.Jobs;
using MpcRen.Register.Server.Options;

var builder = Host.CreateApplicationBuilder(args);

// builder.Services.AddOptions<ServerOptions>("Server");
builder.Services.Configure<ServerOptions>(
    builder.Configuration.GetSection("Server"));
builder.Services.AddOptions<ParticipantsOptions>("Participants");
builder.Services.AddSecretSharingServices();
builder.Services.AddNetworkServices();
builder.Services.AddCommandFactory();
builder.Services.AddHostedService<NetworkConnectionJob>();


using var host = builder.Build();

await host.RunAsync();