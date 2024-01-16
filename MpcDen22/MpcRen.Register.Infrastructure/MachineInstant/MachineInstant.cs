using System.Numerics;
using MpcRen.Register.Infrastructure.MachineInstant.Models;
// ReSharper disable InconsistentNaming

namespace MpcRen.Register.Infrastructure.MachineInstant;

public class MachineInstant : IMachineInstant
{
    private readonly int Name;

    private State CurrentState;
    //Part-ts
    private readonly HashSet<int> Participants;
    private BigInteger Prime = 11441180254372124519;

    public MachineInstant(int name)
    {
        Name = name;
        Participants = new HashSet<int>();
        CurrentState = State.WaitForParticipants;
    }
    
    public int CurrentHostId()
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

    public void ConnectParticipant(int id)
    {
        Participants.Add(id);
        if (IsConnectionsFull())
        {
            CurrentState = State.Active;
        }
    }
}