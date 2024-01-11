namespace MpcDen22.Infrastructure.Checks;

public interface ICheckService
{
    Task<bool> IsSameLogin(string login);
    Task<bool> IsSameShares(List<string> shares);
}