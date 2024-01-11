using System.Numerics;
using Microsoft.Extensions.Logging;
using MpcDen22.Infrastructure.Checks;
using MpcDen22.Infrastructure.CommonModels;

namespace MpcDen22.Infrastructure.Protocol;

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

    public async Task RunProtocolExecution(List<string> shares, string login,
        RegistrationProtocolType registrationProtocolType,
        int shareType)
    {
        _logger.LogInformation("StartingProtocol on {HostName}, with params: {Shares}, {Login},{RegType},{ShareType}",
            _machineInstant.CurrentHostName(), shares, login, registrationProtocolType, shareType);

        if (!await _checkService.IsSameLogin(login) || !await _checkService.IsSameShares(shares))
        {
            await AbortProtocol();
            return;
        };
        
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

    private Task<ProtocolResult> CheckPasswordIncludeSymbolGroups(List<string> shares, params HashSet<string>[] symbolGroups)
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

    private async Task AbortProtocol()
    {
        throw new NotImplementedException();
    }

    private bool CheckLoginExists(string login)
    {
        throw new NotImplementedException();
    }
    public  BigInteger GenerateShareCode(string input, BigInteger q)
    {
        char[] charArray = input.ToCharArray();
        int l = charArray.Length;
        BigInteger result = 0;

        for (int i = 0; i < l; i++)
        {
            result += charArray[i] * BigInteger.Pow(2, 16 * i);
        }

        result = BigInteger.ModPow(result, 1, q);  // Вычисление остатка от деления на q

        return result;
    }
}