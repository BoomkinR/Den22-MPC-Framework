// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MpcRen.Register.Infrastructure.Extensions;
using MpcRen.Register.Server.Options;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddOptions<InstanceOptions>("Server");
builder.Services.AddSecretSharingServices();


using var host = builder.Build();

await host.RunAsync();


const int numberOfMults = 4;
var n = numberOfMults;
var t = (n - 1) / 3;


Console.WriteLine("========================================");
Console.WriteLine($"Running multiplication benchmark with N {n} and #mults {numberOfMults}");
Console.WriteLine("========================================");