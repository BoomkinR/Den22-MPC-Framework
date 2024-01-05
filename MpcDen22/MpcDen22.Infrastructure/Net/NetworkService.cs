namespace MpcDen22.Infrastructure.Net;

public class NetworkService: INetworkService
{
    private readonly IOptions<> 
    
    public Task SendMessage(string msg, int partyIndex)
    {
        throw new NotImplementedException();
    }

    public Task<bool> InitializeAllParties(int currentHostId)
    {
        throw new NotImplementedException();
    }
}