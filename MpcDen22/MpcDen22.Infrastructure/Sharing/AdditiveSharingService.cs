using System.Runtime.InteropServices;
using System.Text.Json;
using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Sharing;

public static class AdditiveSharing
{
    public static List<RingElement?> ShareAdditive(RingElement secret, int n, PRG prg)
    {
        if (n <= 0)
            throw new ArgumentException("Cannot create additive sharing for 0 people.");

        var elementSize = ByteSize<RingElement>();
        var shares = new List<RingElement?>(n);
        var buf = prg.Next(elementSize * n);

        for (var i = 0; i < n - 1; ++i)
        {
            var share = FromBytes(buf.Skip(i * elementSize).Take(elementSize).ToArray());
            shares.Add(share);
        }

        var lastShare = Subtract(secret, VectorSum(shares));
        shares.Add(lastShare);

        return shares;
    }

    private static int ByteSize<RingElement>()
    {
        return Marshal.SizeOf<RingElement>();
    }

    private static RingElement? FromBytes(byte[] bytes)
    {
        return JsonSerializer.Deserialize<RingElement>(bytes);
    }

    private static RingElement Subtract(RingElement a, RingElement b)
    {
        dynamic dynA = a;
        dynamic dynB = b;
        return dynA - dynB;
    }

    private static RingElement VectorSum(IEnumerable<RingElement> vector)
    {
        dynamic sum = default(RingElement);

        foreach (var element in vector)
        {
            dynamic dynElement = element;
            sum += dynElement;
        }

        return sum;
    }
}

public class PRG
{
    // Implement your PRG class logic here
    // ...

    public byte[] Next(int size)
    {
        // Implement the logic to generate pseudo-random bytes
        // Return a byte array of the specified size
        throw new NotImplementedException();
    }
}