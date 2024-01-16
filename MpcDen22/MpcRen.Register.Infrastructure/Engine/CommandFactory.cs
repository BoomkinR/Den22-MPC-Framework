namespace MpcRen.Register.Infrastructure.Engine;

public class CommandFactory: ICommandFactory
{
    public Type GetTypeByNumber(int typeNumber)
    {
        return typeNumber switch
        {
            1 => typeof(CommandFactory),
            _ => typeof(Console)
        };
    }
}