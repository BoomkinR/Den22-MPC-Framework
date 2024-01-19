using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using MpcRen.Register.Infrastructure.Engine;
using MpcRen.Register.Infrastructure.Engine.Commands;
using MpcRen.Register.Server.Options;

namespace MpcRen.Register.Infrastructure.Net;

public class NetworkService : INetworkService
{
    private readonly ICommandFactory _commandFactory;
    private readonly IProcessorCommandController _processorCommandController;
    private readonly ParticipantsOptions _participantsOptions;

    public NetworkService(ICommandFactory commandFactory, IOptions<ParticipantsOptions> participantsOptions)
    {
        _commandFactory = commandFactory;
        _participantsOptions = participantsOptions.Value;
    }


    public async Task SendMessage(string msg)
    {
        foreach (var participant in _participantsOptions.Participants)
        {
            var serverIp = participant.Address;
            var serverPort = participant.Port;

            try
            {
                TcpClient client = new TcpClient();

                // Connect to the server
                await client.ConnectAsync(serverIp, serverPort);

                // Convert the message to bytes
                byte[] data = Encoding.UTF8.GetBytes(msg);

                // Get the network stream from the client
                NetworkStream stream = client.GetStream();

                // Write the message to the stream
                await stream.WriteAsync(data, 0, data.Length);

                // Close the connection
                client.Close();
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }

    public async Task<ComputeCheckResponse> SendCommand(ComputeCheckRequest request)
    {
        foreach (var participant in _participantsOptions.Participants)
        {
            TcpClient client = new TcpClient();
            try
            {
                var serverIp = participant.Address;
                var serverPort = participant.Port;
                // Connect to the server
                await client.ConnectAsync(serverIp, serverPort);

                // Convert the message to bytes
                byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(request));

                // Get the network stream from the client
                NetworkStream stream = client.GetStream();

                // Write the message to the stream
                await stream.WriteAsync(data, 0, data.Length);

                // Read the response from the stream
                byte[] buffer = new byte[1024]; // Adjust the buffer size as needed
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                // Deserialize the response into an object
                string responseJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                var responseObject = JsonSerializer.Deserialize<ComputeCheckResponse>(responseJson);

                // Process the responseObject as needed
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                // Close the connection
                client.Close();
            }
        }

        throw new Exception();
    }

    public async Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken)
    {
        var buffer = new byte[256];
        _ = await stream.ReadAsync(buffer, cancellationToken);

        var data = Encoding.ASCII.GetString(buffer);
        var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(data);
        var rootElement = jsonDocument?.RootElement;
        var typeNumber = rootElement?.GetProperty("type");
        var objectElement = rootElement?.GetProperty("object");
        string responseString = string.Empty;

        switch (typeNumber?.GetInt32())
        {
            case 1:
                var initializeCommand = objectElement?.Deserialize<InitializeHostRequest>();
                var response = await _processorCommandController.InitializeComponent(initializeCommand!);
                responseString = JsonSerializer.Serialize(response);
                break;
            default:
                break;
        }


        var bytes = Encoding.ASCII.GetBytes(responseString);
        await stream.WriteAsync(bytes, cancellationToken);
    }
}