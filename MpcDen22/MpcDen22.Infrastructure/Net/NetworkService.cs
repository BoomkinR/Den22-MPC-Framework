namespace MpcDen22.Infrastructure.Net;

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

    public Task ReadMessage(Stream clientStream)
    {
        throw new NotImplementedException();
    }
    }
}