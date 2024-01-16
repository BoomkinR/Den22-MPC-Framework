using MpcRen.Register.Infrastructure.CommonModels;

namespace MpcRen.Register.Infrastructure.Sharing.Models;

public struct RandomShare
{
    public Replicator<Mp61> RepShare { get; set; }
    public Mp61 AddShare { get; set; }
    public List<Replicator<Mp61>> RepAddShares { get; set; }
}