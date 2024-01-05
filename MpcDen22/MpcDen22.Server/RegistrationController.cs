using Microsoft.AspNetCore.Mvc;
using MpcDen22.Infrastructure.CommonModels;
using MpcDen22.Infrastructure.Protocol;

namespace MpcDen22.Server;

[ApiController]
[Route("api/registration")]
public class RegistrationController : ControllerBase
{
    private readonly IProtocolService _protocolService;

    public RegistrationController(IProtocolService protocolService)
    {
        _protocolService = protocolService;
    }

    [HttpPost("register")]
    public Task Register([FromBody] RegisterRequest registerRequest)
    {
        _protocolService.RunProtocolExecution(registerRequest.SecretShares, registerRequest.Login,
            RegistrationProtocolType.Registration, registerRequest.ShareType);
        return Task.CompletedTask;
    }

    [HttpPost("change-password")]
    public Task ChangePassword([FromBody] RegisterRequest registerRequest)
    {
        _protocolService.RunProtocolExecution(registerRequest.SecretShares, registerRequest.Login,
            RegistrationProtocolType.ChangePassword, registerRequest.ShareType);
        return Task.CompletedTask;
    }
}