using System.Text.Json.Serialization;

namespace MpcRen.Register.Infrastructure.Engine.Commands;

public class InitializeHostResponse
{
    [JsonPropertyName("Id")]
    public int Id {get;set;}
}