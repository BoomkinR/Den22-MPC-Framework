using System.Numerics;

namespace MpcRen.Register.Infrastructure.Checks;

public interface ICheckService
{
    Task<bool> IsSameLogin(string login);
    Task<bool> IsSameShares(BigInteger[] shares);
}