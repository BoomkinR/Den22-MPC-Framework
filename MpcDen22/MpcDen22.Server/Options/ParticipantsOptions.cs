namespace MpcDen22.Server.Options;

public class ParticipantsOptions
{
    public List<Participant> Participants { get; set; }

    public class Participant
    {
        public string Address { get; set; }
    }
}