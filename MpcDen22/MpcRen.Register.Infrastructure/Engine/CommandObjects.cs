using System.Text.Json.Serialization;

namespace MpcRen.Register.Infrastructure.Engine;

public class CommandObjects
{
    [JsonPropertyName("type")]
    public int Type { get; set; }

    [JsonPropertyName("object")] 
    public string Object { get; set; } = string.Empty;
}