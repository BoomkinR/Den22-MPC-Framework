using System.Numerics;
using Microsoft.Extensions.Options;
using MpcRen.Register.Infrastructure.MachineInstant;
using MpcRen.Register.Infrastructure.MachineInstant.Models;
using MpcRen.Register.Server.Options;

// ReSharper disable InconsistentNaming

namespace MpcRen.Register.Server;

public class MachineInstant : IMachineInstant
{
    private readonly int Id;
    private State CurrentState;
    //Part-ts
    private readonly HashSet<int> Participants;
    private readonly BigInteger Prime = 11441180254372124519;
    

    public MachineInstant(IOptions<ServerOptions> serverOptions)
    {
        Id = serverOptions.Value.Number;
        Participants = new HashSet<int>();
        CurrentState = State.WaitForParticipants;
    }
    
    public int CurrentHostId()
    {
        return Id;
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

    public BigInteger GetPrime(int id)
    {
        return Prime;
    }
}