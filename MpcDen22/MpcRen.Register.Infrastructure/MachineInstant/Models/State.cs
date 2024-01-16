namespace MpcRen.Register.Infrastructure.MachineInstant.Models;

public enum State
{
    WaitForParticipants,
    Active,
    InProtocolExecution,
    Error,
    Aborted
}