namespace MpcRen.Register.Infrastructure.Engine;

public interface ICommandFactory
{
    Type GetTypeByNumber(int typeNumber);
}