namespace MpcDen22.Infrastructure.Net;

public interface INetworkService
{
    Task SendMessage(string msg, int partyIndex);

    Task<bool> InitializeParticipant(int Id, string address);
}