using System.Numerics;
using Microsoft.Extensions.Logging;
using MpcRen.Register.Infrastructure.Checks;
using MpcRen.Register.Infrastructure.CommonModels;
using MpcRen.Register.Infrastructure.MachineInstant;

namespace MpcRen.Register.Infrastructure.Protocol;

public class ProtocolService : IProtocolService
{
    private readonly ILogger<ProtocolService> _logger;
    private readonly IMachineInstant _machineInstant;
    private readonly ICheckService _checkService;

    public ProtocolService(ILogger<ProtocolService> logger, IMachineInstant machineInstant, ICheckService checkService)
    {
        _logger = logger;
        _machineInstant = machineInstant;
        _checkService = checkService;
    }

    public async Task RunProtocolExecution(BigInteger[] shares, string login,
        RegistrationProtocolType registrationProtocolType,
        int shareType)
    {
        _logger.LogInformation("StartingProtocol on {HostName}, with params: {Shares}, {Login},{RegType},{ShareType}",
            _machineInstant.CurrentHostId(), shares, login, registrationProtocolType, shareType);

        if (!await _checkService.IsSameLogin(login) || !await _checkService.IsSameShares(shares))
        {
            await AbortProtocol();
            return;
        }

        if (registrationProtocolType == RegistrationProtocolType.ChangePassword)
        {
            var isLoginExists = CheckLoginExists(login);
            if (!isLoginExists)
            {
                _logger.LogError("Aborting protocol cause of LoginExistsError");
                await AbortProtocol();
                await SendErrorCallback(new ProtocolResult
                {
                    Result = 1,
                    TestNumber = TestNumbers.Login
                });
            }
        }

        var passwordLengthCheck = CheckPasswordLength(shares);
        var passwordUpperCaseExists = CheckPasswordIncludeSymbolGroups(shares, SymbolGroups.UpperCaseEng,
            SymbolGroups.LowerCaseEng, SymbolGroups.Specials, SymbolGroups.Specials);

        await Task.WhenAll(passwordLengthCheck, passwordUpperCaseExists);
    }

    private Task<ProtocolResult> CheckPasswordIncludeSymbolGroups(BigInteger[] shares,
        params HashSet<string>[] symbolGroups)
    {
        throw new NotImplementedException();
    }

    private Task<ProtocolResult> CheckPasswordLength(BigInteger[] shares)
    {
        throw new NotImplementedException();
    }

    private async Task SendErrorCallback(ProtocolResult protocolResult)
    {
        throw new NotImplementedException();
    }

    private async Task AbortProtocol()
    {
        throw new NotImplementedException();
    }

    private bool CheckLoginExists(string login)
    {
        throw new NotImplementedException();
    }
    
    
}