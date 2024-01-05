namespace MpcDen22.Infrastructure.Net;

public interface INetworkService
{
    Task SendMessage(string msg, int partyIndex);

    Task<bool> InitializeAllParties(int currentHostId);
}