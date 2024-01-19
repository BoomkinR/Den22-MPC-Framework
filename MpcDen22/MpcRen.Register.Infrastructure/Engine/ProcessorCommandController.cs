using Microsoft.Extensions.Options;
using MpcRen.Register.Infrastructure.Engine.Commands;
using MpcRen.Register.Infrastructure.MachineInstant;
using MpcRen.Register.Server.Options;

namespace MpcRen.Register.Infrastructure.Engine;

public class ProcessorCommandController : IProcessorCommandController
{
    private readonly IMachineInstant _machineInstant;
    private readonly InstanceOptions _instanceOptions;

    public ProcessorCommandController(IMachineInstant machineInstant,
        IOptions<InstanceOptions> instanceOptions)
    {
        _machineInstant = machineInstant;
        _instanceOptions = instanceOptions.Value;
    }

    public Task<InitializeHostResponse> InitializeComponent(InitializeHostRequest initializeHostRequest)
    {
        if (!_machineInstant.IsConnectionsFull())
        {
            _machineInstant.ConnectParticipant(initializeHostRequest.Id);
        }
        else
        {
            Console.WriteLine("All hosts connected");
        }

        return Task.FromResult(new InitializeHostResponse
        {
            Id = _machineInstant.CurrentHostId()
        });
    }
}