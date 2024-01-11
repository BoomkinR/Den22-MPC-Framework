// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Hosting;
using MpcDen22.Infrastructure;
using MpcDen22.Infrastructure.CommonModels;
using MpcDen22.Infrastructure.Input;
using MpcDen22.Infrastructure.Sharing;

var builder = Host.CreateApplicationBuilder(args);


using IHost host = builder.Build();

await host.RunAsync();


const int numberOfMults = 4;
var n = numberOfMults;
var t = (n - 1) / 3;


Console.WriteLine("========================================");
Console.WriteLine($"Running multiplication benchmark with N {n} and #mults {numberOfMults}");
Console.WriteLine("========================================");

var replicator = new Replicator<Mp61>(n, t);
var correlator = new (id, replicator);
var manipulator = new ShrManipulator(id, t, n);

// установитиь связь сетевую
var instance = new Mult()
// Ждать input
// Вызов логики вычисления А последовательно каждой
// Хешировать
// Дать ответ