using System.Text.Json.Serialization;

namespace MpcRen.Register.Server;

public class RegisterRequest
{
    [JsonPropertyName("secrets")] public List<string> SecretShares { get; set; }

    [JsonPropertyName("login")] public string Login { get; set; }

    [JsonPropertyName("shareType")] public int ShareType { get; set; }
}