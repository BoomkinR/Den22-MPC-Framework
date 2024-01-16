using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using MpcRen.Register.Infrastructure.Engine;
using MpcRen.Register.Infrastructure.Engine.Commands;

namespace MpcRen.Register.Infrastructure.Net;

public class NetworkService : INetworkService
{
    private readonly ICommandFactory _commandFactory;
    private readonly IProcessorCommandController _processorCommandController;

    public NetworkService(ICommandFactory commandFactory)
    {
        _commandFactory = commandFactory;
    }

    public Task SendMessage(string msg, int partyIndex)
    {
        throw new NotImplementedException();
    }

    public async Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken)
    {
        var buffer = new byte[256];
        _ = await stream.ReadAsync(buffer, cancellationToken);

        var data = Encoding.ASCII.GetString(buffer);
        var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(data);
        var rootElement = jsonDocument?.RootElement;
        var typeNumber = rootElement?.GetProperty("type");
        var objectElement = (rootElement?.GetProperty("object"));
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