
using System.Text.Json.Serialization;

namespace MpcRen.Register.Infrastructure.Engine.Commands;

public class InitializeHostRequest
{
    [JsonPropertyName("Id")]
    public int Id {get;set;}
}