using MpcRen.Register.Infrastructure.Engine.Commands;

namespace MpcRen.Register.Infrastructure.Engine;

public interface IProcessorCommandController
{
    Task<InitializeHostResponse> InitializeComponent(InitializeHostRequest initializeHostRequest);
}