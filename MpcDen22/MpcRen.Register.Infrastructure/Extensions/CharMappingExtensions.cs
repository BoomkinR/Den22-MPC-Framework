using System.Numerics;
using System.Text;

namespace MpcRen.Register.Infrastructure.Extensions;

// works just for ENG chars
public static class CharMappingExtensions
{
    public static BigInteger ToAsci2BigInt(this char c)
    {
        var b = Convert.ToByte(c);
        return new BigInteger(b);
    }

    public static char ToChar2Asci(this BigInteger b)
    {
        return Convert.ToChar((int) b);
    }
}