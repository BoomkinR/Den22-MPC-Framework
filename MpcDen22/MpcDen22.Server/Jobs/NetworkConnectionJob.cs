using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using MpcDen22.Infrastructure;

namespace MpcDen22.Server.Jobs;

public class NetworkConnectionJob : IHostedService
{
    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly IMachineInstant _machineInstant;
    public NetworkConnectionJob(IMachineInstant machineInstant)
    {
        _machineInstant = machineInstant;
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
        try
        {
            // Set the TcpListener on port 13000.
            var port = 13000;
            var localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            var buffer = new byte[256];

            // Enter the listening loop.
            while (!cancellationToken.IsCancellationRequested)
            {
                Console.Write("Waiting for a connection... ");

                while (!_machineInstant.IsConnectionsFull()) await _machineInstant.ConnectParticipant();

                // recieve message;
                // Call message controller

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                using var client = await server.AcceptTcpClientAsync(cancellationToken);
                Console.WriteLine("Connected!");

                // Get a stream object for reading and writing
                var stream = client.GetStream();

                _ = await stream.ReadAsync(buffer, cancellationToken);
                int i;

                var data = Encoding.ASCII.GetString(buffer);


                var response = Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(response, 0, response.Length);
                Console.WriteLine("Sent: {0}", data);
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