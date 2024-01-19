namespace MpcRen.Register.Infrastructure;

public class ParticipantsOptions
{
    public List<Participant> Participants { get; set; }

    public class Participant
    {
        public string Address { get; set; }
        public int Port { get; set; } 
    }
}