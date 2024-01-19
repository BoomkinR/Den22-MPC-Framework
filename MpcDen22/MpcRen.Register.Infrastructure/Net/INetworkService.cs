using System.Net.Sockets;
using MpcRen.Register.Infrastructure.Engine.Commands;

namespace MpcRen.Register.Infrastructure.Net;

public interface INetworkService
{
    Task SendMessage(string msg);
    Task<ComputeCheckResponse> SendCommand(ComputeCheckRequest request);

    Task ProcessStream(NetworkStream stream, CancellationToken cancellationToken);
}