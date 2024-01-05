namespace MpcDen22.Infrastructure;

public class MachineInstant: IMachineInstant
{
    public readonly string Name;
    public MachineInstant(string name)
    {
        Name = name;
    }

    public string CurrentHostName()
    {
        return Name;
    }
}