using System.Numerics;

namespace MpcDen22.Infrastructure;

public class MachineInstant : IMachineInstant
{
    private readonly string Name;

    //Part-ts
    private readonly List<int> Participants;
    private BigInteger Prime = 11441180254372124519;

    public MachineInstant(string name)
    {
        Name = name;
        Participants = new List<int>();
    }


    public string CurrentHostId()
    {
        return Name;
    }

    public int ParticipantCount()
    {
        return Participants.Count;
    }

    public bool IsConnectionsFull()
    {
        return Participants.Count >= 3;
    }

    public Task ConnectParticipant()
    {
        throw new NotImplementedException();
    }
}