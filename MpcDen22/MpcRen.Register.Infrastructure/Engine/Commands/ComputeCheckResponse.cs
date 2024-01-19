using System.Numerics;

namespace MpcRen.Register.Infrastructure.Engine.Commands;

public class ComputeCheckResponse
{
    public (BigInteger[], BigInteger[], BigInteger[]) Shares { get; set; }
}