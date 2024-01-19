using System.Numerics;

namespace MpcRen.Register.Client.Network;

public class HttpRenRegClient: HttpClient
{
    public Task SendMessageToServer(string login, (BigInteger[], BigInteger[], BigInteger[]) shares, int serverId)
    {
        
        return Task.CompletedTask;
    }
}