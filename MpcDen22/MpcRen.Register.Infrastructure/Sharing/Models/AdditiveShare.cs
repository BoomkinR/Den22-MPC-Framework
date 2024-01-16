using MpcRen.Register.Infrastructure.CommonModels;

namespace MpcRen.Register.Infrastructure.Sharing.Models;

public struct AdditiveShare
{
    public Mp61 AddShare { get; set; }
    public List<Mp61> RepAddShares { get; set; }
}