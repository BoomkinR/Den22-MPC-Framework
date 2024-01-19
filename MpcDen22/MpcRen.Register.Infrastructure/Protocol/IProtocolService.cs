using System.Numerics;
using MpcRen.Register.Infrastructure.CommonModels;

namespace MpcRen.Register.Infrastructure.Protocol;

public interface IProtocolService
{
    Task RunProtocolExecution(BigInteger[] shares, string login, RegistrationProtocolType registrationProtocolType,
        int shareType);
}