using System.Numerics;
using System.Text.Json.Serialization;

namespace MpcRen.Register.Server;

public class RegisterRequest
{
    [JsonPropertyName("secrets")] public (BigInteger[], BigInteger[],BigInteger[]) SecretShares { get; set; }

    [JsonPropertyName("login")] public string Login { get; set; }

    [JsonPropertyName("shareType")] public int ShareType { get; set; }
}