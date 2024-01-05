using System;
using System.Numerics;

namespace MpcDen22.Infrastructure.CommonModels
{
    public class Mp127 : RingByModElement<BigInteger>
    {

        public Mp127(BigInteger value) : base(BigInteger.Pow(2, 127) - 1, value)
        {
        }
        protected bool Equals(Mp127 other)
        {
            return PrimeValue.Equals(other.PrimeValue) && Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Mp127)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PrimeValue, Value);
        }

        public static BigInteger Reduce(BigInteger x, BigInteger mod)
        {
            return x % mod;
        }

        public static BigInteger AddMod(BigInteger x, BigInteger y, BigInteger mod)
        {
            return (x + y) % mod;
        }

        public static BigInteger SubtractMod(BigInteger x, BigInteger y, BigInteger mod)
        {
            return (x - y + mod) % mod;
        }

        public static BigInteger MultiplyMod(BigInteger x, BigInteger y, BigInteger mod)
        {
            return (x * y) % mod;
        }

        public static BigInteger Negate(BigInteger v, BigInteger mod)
        {
            return (mod - v) % mod;
        }

        public static BigInteger InvertMod(BigInteger v, BigInteger mod)
        {
            return BigInteger.ModPow(v, mod - 2, mod);
        }

        public static bool Equal(Mp127 x, Mp127 y)
        {
            return x.Equals(y);
        }

        public static BigInteger operator +(Mp127 x, BigInteger y)
        {
            return x.Add(y);
        }

        public static BigInteger operator -(Mp127 x, BigInteger y)
        {
            return x.Subtract(y);
        }

        public static BigInteger operator *(Mp127 x, BigInteger y)
        {
            return x.Multiply(y);
        }

        public static BigInteger operator !(Mp127 x)
        {
            return x.Negate();
        }

        public static bool operator ==(Mp127 x, BigInteger y)
        {
            return x.Value.Equals(y);
        }

        public static bool operator !=(Mp127 x, BigInteger y)
        {
            return !(x == y);
        }

        public static bool operator ==(Mp127 x, Mp127 y)
        {
            return x.Equal(y);
        }

        public static bool operator !=(Mp127 x, Mp127 y)
        {
            return !(x == y);
        }
    }

    public static class Mp127Extensions
    {
        public static BigInteger Add(this Mp127 x, BigInteger y)
        {
            return Mp127.AddMod(x.Value, y, x.PrimeValue);
        }

        public static BigInteger Subtract(this Mp127 x, BigInteger y)
        {
            return Mp127.SubtractMod(x.Value, y, x.PrimeValue);
        }

        public static BigInteger Multiply(this Mp127 x, BigInteger y)
        {
            return Mp127.MultiplyMod(x.Value, y, x.PrimeValue);
        }

        public static BigInteger Negate(this Mp127 x)
        {
            return Mp127.Negate(x.Value, x.PrimeValue);
        }

        public static BigInteger Invert(this Mp127 x)
        {
            return Mp127.InvertMod(x.Value, x.PrimeValue);
        }

        public static bool Equal(this Mp127 x, Mp127 y)
        {
            return Mp127.Equal(x, y);
        }
    }
}