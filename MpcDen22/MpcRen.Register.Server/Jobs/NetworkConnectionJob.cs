using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using MpcRen.Register.Infrastructure;
using MpcRen.Register.Infrastructure.MachineInstant;
using MpcRen.Register.Infrastructure.Net;

namespace MpcRen.Register.Server.Jobs;

public class NetworkConnectionJob : IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IMachineInstant _machineInstant;
    private readonly INetworkService _networkService;
    public NetworkConnectionJob(IMachineInstant machineInstant, INetworkService networkService)
    {
        _machineInstant = machineInstant;
        _networkService = networkService;
    }
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    public Task StartAsync(CancellationToken cancellationToken)
    {
        RunNetworkHostedJob(_cancellationTokenSource.Token);
        return Task.CompletedTask;
    }
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed


    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _cancellationTokenSource.CancelAsync();
    }


    private async Task RunNetworkHostedJob(CancellationToken cancellationToken)
    {
        TcpListener server = null;
        // Set the TcpListener on port 13000.
        var port = 13000;
        var localAddr = IPAddress.Parse("127.0.0.1");

        // TcpListener server = new TcpListener(port);
        server = new TcpListener(localAddr, port);

        // Start listening for client requests.
        server.Start();
        try
        {
            // Enter the listening loop.
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Waiting for a connection... ");

                using var client = await server.AcceptTcpClientAsync(cancellationToken);
                var stream = client.GetStream();
                await _networkService.ProcessStream(stream, cancellationToken);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
        }
        finally
        {
            server.Stop();
        }
    }
}