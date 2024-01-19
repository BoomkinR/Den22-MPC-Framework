using System.Numerics;
using MpcRen.Register.Infrastructure.Extensions;

namespace MpcRen.Register.Infrastructure.Sharing;

public class SecretShareService: ISecretShareService
{
    public (BigInteger[] x1, BigInteger[] x2, BigInteger[] x3, BigInteger[] x4) GenerateShares(string input, BigInteger q)
    {
        var charArray = input.ToCharArray();
        var l = charArray.Length;
        BigInteger[] x1 = new BigInteger[l];
        BigInteger[] x2 = new BigInteger[l];
        BigInteger[] x3 = new BigInteger[l];
        BigInteger[] x4 = new BigInteger[l];

        for (var i = 0; i < l; i++)
        {
            x1[i] = GenerateRandomValue(q);
            x2[i] = GenerateRandomValue(q); 
            x3[i] = GenerateRandomValue(q);
            x4[i] = (charArray[i].ToAsci2BigInt() - x1[i] - x2[i] - x3[i]) % q;
        }

        return (x1, x2, x3, x4);
    }

    public string GetValueFromShares(BigInteger[] x1, BigInteger[] x2, BigInteger[] x3, BigInteger[] x4, BigInteger q)
    {
        var l = x1.Length;
        var codes = new char[l];
        for (int i = 0; i < l; i++)
        {
            var result = BigInteger.Abs( (x1[i]+ x2[i] + x3[i] +x4[i]) % q);
            codes[i] = result.ToChar2Asci();
        }
        
        return new string(codes);
    }

    private BigInteger GenerateRandomValue(BigInteger q)
    {
        Random random = new Random();
        // /3 for escape out of BigInteger
        byte[] randomBytes = new byte[q.ToByteArray().LongLength/3];
        random.NextBytes(randomBytes);
        BigInteger randomValue = new BigInteger(randomBytes);
        randomValue %= q;

        return randomValue;
    }
}