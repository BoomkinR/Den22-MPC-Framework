namespace MpcDen22.Infrastructure;

public interface IMachineInstant
{
    string CurrentHostId();
    int ParticipantCount();
    bool IsConnectionsFull();
    Task ConnectParticipant();
}