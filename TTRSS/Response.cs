namespace TTRSS;

using System.Text.Json.Serialization;

public class Response<T> {
    [JsonPropertyName("status")]
    public ResponseStatus Status { get; init; }
    readonly T content;
    [JsonPropertyName("content")]
    public required T Content {
        get {
            this.EnsureSuccess();
            return this.content;
        }
        init => this.content = value;
    }

    void EnsureSuccess() {
        if (this.Status != ResponseStatus.SUCCESS)
            throw new TinyTinyRssException("Request failed");
    }
}

public enum ResponseStatus {
    SUCCESS,
    ERROR,
}