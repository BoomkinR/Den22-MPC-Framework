using System.Numerics;

namespace MpcRen.Register.Infrastructure.Checks;

public class CheckService : ICheckService
{
    public Task<bool> IsSameLogin(string login)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsSameShares(BigInteger[] shares)
    {
        throw new NotImplementedException();
    }
}