using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Protocol;

public interface IProtocolService
{
    Task RunProtocolExecution(List<string> shares, string login, RegistrationProtocolType registrationProtocolType, int shareType);
}