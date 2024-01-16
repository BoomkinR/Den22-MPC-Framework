using MpcRen.Register.Infrastructure.CommonModels;

namespace MpcRen.Register.Infrastructure.Protocol;

public interface IProtocolService
{
    Task RunProtocolExecution(List<string> shares, string login, RegistrationProtocolType registrationProtocolType,
        int shareType);
}