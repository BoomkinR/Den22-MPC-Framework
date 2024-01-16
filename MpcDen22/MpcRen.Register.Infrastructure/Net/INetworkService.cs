using System.Net.Sockets;

namespace MpcRen.Register.Infrastructure.Net;

public interface INetworkService
{
    Task SendMessage(string msg, int partyIndex);

    Task<bool> InitializeParticipant(int Id, string address);

    Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken);
}