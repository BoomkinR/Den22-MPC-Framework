using MpcRen.Register.Infrastructure.Engine.Commands;

namespace MpcRen.Register.Infrastructure.Engine;

public class CommandFactory: ICommandFactory
{
    public Type GetTypeByNumber(int? typeNumber)
    {
        return typeNumber switch
        {
            1 => typeof(InitializeHostRequest),
            _ => typeof(Console)
        };
    }
}