namespace MpcRen.Register.Infrastructure.Net.Options;

public class HostsOptions
{
    public const string Name = "Participants";

    public HostsOptions(Dictionary<string, string> servers, Dictionary<string, string> listener)
    {
        Servers = servers;
        Listener = listener;
    }

    public Dictionary<string, string> Servers { get; set; }
    public Dictionary<string, string> Listener { get; set; }
}