using System.Numerics;

namespace MpcDen22.Infrastructure.CommonModels;

public class Mp61 : RingByModElement<ulong>
{
    public Mp61(ulong value) : base(0x1FFFFFFFFFFFFFFF, value)
    {
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((Mp61)obj);
    }

    public static ulong Reduce(ulong x, ulong mod)
    {
        return x % mod;
    }

    public static ulong AddMod(ulong x, ulong y, ulong mod)
    {
        return (x + y) % mod;
    }

    public static ulong SubtractMod(ulong x, ulong y, ulong mod)
    {
        return (x - y + mod) % mod;
    }

    public static ulong MultiplyMod(ulong x, ulong y, ulong mod)
    {
        return x * y % mod;
    }

    public static ulong Negate(ulong v, ulong mod)
    {
        return (mod - v) % mod;
    }

    public static ulong InvertMod(ulong v, ulong mod)
    {
        // Пожалуйста, учтите, что этот метод может неэффективно работать для больших значений ulong.
        // В реальных случаях лучше использовать другие библиотеки для работы с большими числами.
        return (ulong)BigInteger.ModPow(v, mod - 2, mod);
    }

    public static bool Equal(Mp61 x, Mp61 y)
    {
        return x.Equals(y);
    }

    public static ulong operator +(Mp61 x, ulong y)
    {
        return x.Add(y);
    }

    public static ulong operator -(Mp61 x, ulong y)
    {
        return x.Subtract(y);
    }

    public static ulong operator *(Mp61 x, ulong y)
    {
        return x.Multiply(y);
    }

    public static ulong operator !(Mp61 x)
    {
        return x.Negate();
    }

    public static bool operator ==(Mp61 x, ulong y)
    {
        return x.Value.Equals(y);
    }

    public static bool operator !=(Mp61 x, ulong y)
    {
        return !(x == y);
    }

    public static bool operator ==(Mp61 x, Mp61 y)
    {
        return x.Equal(y);
    }

    public static bool operator !=(Mp61 x, Mp61 y)
    {
        return !(x == y);
    }
}

public static class Mp61Extensions
{
    public static ulong Add(this Mp61 x, ulong y)
    {
        return Mp61.AddMod(x.Value, y, x.PrimeValue);
    }

    public static ulong Subtract(this Mp61 x, ulong y)
    {
        return Mp61.SubtractMod(x.Value, y, x.PrimeValue);
    }

    public static ulong Multiply(this Mp61 x, ulong y)
    {
        return Mp61.MultiplyMod(x.Value, y, x.PrimeValue);
    }

    public static ulong Negate(this Mp61 x)
    {
        return Mp61.Negate(x.Value, x.PrimeValue);
    }

    public static ulong Invert(this Mp61 x)
    {
        return Mp61.InvertMod(x.Value, x.PrimeValue);
    }

    public static bool Equal(this Mp61 x, Mp61 y)
    {
        return Mp61.Equal(x, y);
    }
}