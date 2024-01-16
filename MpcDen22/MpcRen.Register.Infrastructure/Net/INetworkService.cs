using System.Net.Sockets;

namespace MpcRen.Register.Infrastructure.Net;

public interface INetworkService
{
    Task SendMessage(string msg, int partyIndex);

    Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken);
}