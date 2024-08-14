namespace TTRSS;

using System.Text.Json.Serialization;

public class RequestBase {
    [JsonPropertyName("op")]
    public string Op { get; }

    protected RequestBase(string op) {
        this.Op = op ?? throw new ArgumentNullException(nameof(op));
    }
}

public class AuthorizedRequest: RequestBase {
    [JsonPropertyName("sid")]
    public required string SessionID { get; init; }

    protected AuthorizedRequest(string op): base(op) { }
}