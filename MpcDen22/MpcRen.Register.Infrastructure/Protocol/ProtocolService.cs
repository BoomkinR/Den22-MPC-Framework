using System.Numerics;
using Microsoft.Extensions.Logging;
using MpcRen.Register.Infrastructure.Checks;
using MpcRen.Register.Infrastructure.CommonModels;
using MpcRen.Register.Infrastructure.Engine.Commands;
using MpcRen.Register.Infrastructure.Extensions;
using MpcRen.Register.Infrastructure.MachineInstant;
using MpcRen.Register.Infrastructure.Net;

namespace MpcRen.Register.Infrastructure.Protocol;

public class ProtocolService : IProtocolService
{
    private readonly ILogger<ProtocolService> _logger;
    private readonly IMachineInstant _machineInstant;
    private readonly ICheckService _checkService;
    private readonly INetworkService _networkService;

    public ProtocolService(ILogger<ProtocolService> logger, IMachineInstant machineInstant, ICheckService checkService, INetworkService networkService)
    {
        _logger = logger;
        _machineInstant = machineInstant;
        _checkService = checkService;
        _networkService = networkService;
    }

    public async Task RunProtocolExecution((BigInteger[], BigInteger[], BigInteger[]) shares, string login,
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
        var passwordUpperCaseExists = await CheckPasswordIncludeSymbolGroups(shares, SymbolGroups.UpperCaseEng,
            SymbolGroups.LowerCaseEng, SymbolGroups.Specials, SymbolGroups.Specials);

    }

    private async Task<ProtocolResult> CheckPasswordIncludeSymbolGroups((BigInteger[], BigInteger[], BigInteger[]) shares,
        params HashSet<string>[] symbolGroups)
    {
            // Получение пароля из долей
            var currentHost = _machineInstant.CurrentHostId();

            foreach (var symbolGroup in symbolGroups)
            {
                foreach (var symbol in symbolGroup)
                {
                    BigInteger[] x = shares.Item1;
                    BigInteger[] xi = new BigInteger[x.Length];
                    for (int i = 0; i < x.Length; i++)
                    {
                        xi[i] = x[i] - symbol[0].ToAsci2BigInt();
                    }
                    await _networkService.SendCommand(new ComputeCheckRequest(){Shares = (xi,shares.Item2,shares.Item3)});
                }
                    
            }


            // Если проверки пройдены успешно
            return new ProtocolResult
            {
                Result = 0
            };
        
    }

    private ProtocolResult CheckPasswordLength((BigInteger[], BigInteger[], BigInteger[]) shares)
    {
        return new ProtocolResult
        {
            Result = shares.Item1.Length is > 8 and < 64 ? 0 : 1,
            TestNumber = 1
        };
    }

    private async Task SendErrorCallback(ProtocolResult protocolResult)
    {
        await _networkService.SendMessage("Error");
    }

    private async Task AbortProtocol()
    {
        await _networkService.SendMessage("Error");
    }

    private bool CheckLoginExists(string login)
    {
        Console.WriteLine("Login not exists");
        return false;
    }
    
    private  BigInteger ComputeGFunction(BigInteger[] xValues)
    {
        var l = xValues.Length;
        BigInteger result = 0;

        for (var i = 0; i < l; i++)
        {
            result = (result + xValues[i] * BigInteger.Pow(2, 16 * (l - i - 1))) % _machineInstant.GetPrime();
        }

        return result;
    }

    
    
}