namespace MpcRen.Register.Server.MachineInstant;

public interface IMachineInstant
{
    string CurrentHostId();
    int ParticipantCount();
    bool IsConnectionsFull();
    Task ConnectParticipant();
}