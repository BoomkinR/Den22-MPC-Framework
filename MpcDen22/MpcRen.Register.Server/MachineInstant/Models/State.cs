namespace MpcRen.Register.Server.MachineInstant.Models;

public enum State
{
    WaitForParticipants,
    Active,
    InProtocolExecution,
    Error,
    Aborted
}