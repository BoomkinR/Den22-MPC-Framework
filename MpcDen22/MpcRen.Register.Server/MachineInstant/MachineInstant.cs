using System.Numerics;
using MpcRen.Register.Server.MachineInstant.Models;

namespace MpcRen.Register.Server.MachineInstant;

public class MachineInstant : IMachineInstant
{
    private readonly string name;

    private readonly State CurrentState;
    //Part-ts
    private readonly List<int> Participants;
    private BigInteger Prime = 11441180254372124519;

    public MachineInstant(string name)
    {
        this.name = name;
        Participants = new List<int>();
        CurrentState = State.WaitForParticipants;
    }


    public string CurrentHostId()
    {
        return name;
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