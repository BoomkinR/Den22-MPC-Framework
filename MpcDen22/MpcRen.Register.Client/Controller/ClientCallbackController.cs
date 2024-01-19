using Microsoft.AspNetCore.Mvc;

namespace MpcRen.Register.Client.Controller;

[ApiController]
[Route("client/callback")]
public class ClientCallbackController : ControllerBase
{

    [HttpPost("computation-result")]
    public IActionResult ComputationResult()
    {
        
        return Ok();
    }
}