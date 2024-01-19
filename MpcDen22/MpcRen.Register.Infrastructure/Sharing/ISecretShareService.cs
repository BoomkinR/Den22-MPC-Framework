using System.Numerics;

namespace MpcRen.Register.Infrastructure.Sharing;

public interface ISecretShareService
{
    (BigInteger[] x1, BigInteger[] x2, BigInteger[] x3, BigInteger[] x4) GenerateShares(string input, BigInteger q);
    
    string GetValueFromShares(BigInteger[] x1, BigInteger[] x2, BigInteger[] x3, BigInteger[] x4, BigInteger q);
}