using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MpcRen.Register.Infrastructure.Net;

public class NetworkService : INetworkService
{
    public Task SendMessage(string msg, int partyIndex)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InitializeParticipant(int Id, string address)
    {
        // посылает запрос если нет в списке ни в каком ( 2 списка отправленные кому и принятые от кого - сделать через queue)
        // Отправляет ему запрос на подключение
    }

    public async Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken)
    {
        
        // Buffer for reading data
        var buffer = new byte[256];
        _ = await stream.ReadAsync(buffer, cancellationToken);

        var data = Encoding.ASCII.GetString(buffer);
        var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(data);
        var param1 = jsonDocument?.RootElement.GetProperty("type");
        

        var response = Encoding.ASCII.GetBytes(data);

        // Send back a response.
        stream.Write(response, 0, response.Length);
    }

    public Task ReadMessage(Stream clientStream)
    {
        throw new NotImplementedException();
    }
    }
}