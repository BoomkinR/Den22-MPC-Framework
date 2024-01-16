using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Sharing.Models;

public struct AdditiveShare
{
    public Mp61 AddShare { get; set; }
    public List<Mp61> RepAddShares { get; set; }
}