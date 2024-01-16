namespace MpcRen.Register.Infrastructure.MachineInstant;

public interface IMachineInstant
{
    int CurrentHostId();
    int ParticipantCount();
    bool IsConnectionsFull();
    void ConnectParticipant(int id);
}