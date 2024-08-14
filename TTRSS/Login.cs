namespace TTRSS;

using System.Text.Json.Serialization;

public class LoginRequest: RequestBase {
    [JsonPropertyName("user")]
    public required string User { get; init; }
    [JsonPropertyName("password")]
    public required string Password { get; init; }

    public LoginRequest(): base("login") { }
}

public class LoginResponse {
    [JsonPropertyName("session_id")]
    public required string SessionID { get; init; }
}