using System.Numerics;

namespace MpcRen.Register.Client.Network;

public interface IHttpRenRegClient
{
    Task SendMessageToServer(string login, (BigInteger[], BigInteger[], BigInteger[]) shares, int serverId);
}