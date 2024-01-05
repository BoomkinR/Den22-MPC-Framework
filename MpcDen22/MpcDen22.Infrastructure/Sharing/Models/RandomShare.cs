namespace MpcDen22.Infrastructure.Sharing.Models;

public struct RandomShare
{
    public Shr RepShare { get; set; }
    public Field AddShare { get; set; }
    public List<Shr> RepAddShares { get; set; }
}