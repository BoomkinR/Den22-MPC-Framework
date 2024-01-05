using Microsoft.Extensions.Logging;
using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Protocol;

public class ProtocolService : IProtocolService
{
    private readonly ILogger<ProtocolService> _logger;
    private readonly IMachineInstant _machineInstant;

    public ProtocolService(ILogger<ProtocolService> logger, IMachineInstant machineInstant)
    {
        _logger = logger;
        _machineInstant = machineInstant;
    }

    public async Task RunProtocolExecution(List<string> shares, string login,
        RegistrationProtocolType registrationProtocolType,
        int shareType)
    {
        _logger.LogInformation("StartingProtocol on {HostName}, with params: {Shares}, {Login},{RegType},{ShareType}",
            _machineInstant.CurrentHostName(), shares, login, registrationProtocolType, shareType);

        var isLoginExists = CheckLoginExists(login);
        if (registrationProtocolType == RegistrationProtocolType.Registration && isLoginExists ||
            registrationProtocolType == RegistrationProtocolType.ChangePassword && !isLoginExists)
        {
            _logger.LogError("Aborting protocol cause of LoginExistsError");
            await AbbortProtocol();
            await SendErrorCallback(new ProtocolResult
            {
                Result = 1,
                TestNumber = TestNumbers.Login
            });
        }

        var passwordLengthCheck = CheckPasswordLength(shares);
        var passwordUpperCaseExists = CheckPasswordUpperCaseExists(shares);
        var passwordLowerCaseExists = CheckPasswordLowerCaseExists(shares);
        var passwordNumeralExists = CheckPasswordNumeralExists(shares);
        var passwordSpecialSymbolExists = CheckPasswordSpecialSymbolExists(shares);

        await Task.WhenAll(passwordLengthCheck, passwordUpperCaseExists, passwordLowerCaseExists, passwordNumeralExists,
            passwordSpecialSymbolExists);
    }

    private Task<ProtocolResult> CheckPasswordNumeralExists(List<string> shares)
    {
        throw new NotImplementedException();
    }

    private Task<ProtocolResult> CheckPasswordSpecialSymbolExists(List<string> shares)
    {
        throw new NotImplementedException();
    }

    private Task<ProtocolResult> CheckPasswordLowerCaseExists(List<string> shares)
    {
        throw new NotImplementedException();
    }

    private Task<ProtocolResult> CheckPasswordUpperCaseExists(List<string> shares)
    {
        throw new NotImplementedException();
    }

    private Task<ProtocolResult> CheckPasswordLength(List<string> shares)
    {
        throw new NotImplementedException();
    }

    private async Task SendErrorCallback(ProtocolResult protocolResult)
    {
        throw new NotImplementedException();
    }

    private async Task AbbortProtocol()
    {
        throw new NotImplementedException();
    }

    private bool CheckLoginExists(string login)
    {
        throw new NotImplementedException();
    }
}