namespace TTRSS;

using System.Text.Json.Serialization;

public class GetHeadlines: AuthorizedRequest {
    [JsonPropertyName("feed_id")]
    public required int FeedID { get; init; }
    [JsonPropertyName("limit")]
    public required int Limit { get; init; }
    [JsonPropertyName("skip")]
    public required int Skip { get; init; }
    [JsonPropertyName("view_mode")]
    public required string ViewMode { get; init; }

    public GetHeadlines(): base("getHeadlines") { }

    public static class ViewModes {
        public const string ALL_ARTICLES = "all_articles";
        public const string UNREAD = "unread";
    }
}

public class Headline {
    [JsonPropertyName("id")]
    public required int ID { get; init; }
    [JsonPropertyName("title")]
    public required string Title { get; init; }
    [JsonPropertyName("feed_title")]
    public string? FeedTitle { get; init; }
    [JsonPropertyName("tags")]
    public required string[] Tags { get; init; }
    [JsonPropertyName("labels")]
    public required string[] Labels { get; init; }
    [JsonPropertyName("author")]
    public string? Author { get; init; }
    [JsonPropertyName("link")]
    public required Uri Link { get; init; }

    public override string ToString() {
        var result = new System.Text.StringBuilder();
        foreach (string tag in this.Tags.Where(NonEmpty)) {
            result.Append('[');
            result.Append(tag);
            result.Append("] ");
        }

        foreach (string label in this.Labels.Where(NonEmpty)) {
            result.Append('{');
            result.Append(label);
            result.Append("} ");
        }

        result.Append(this.Title);
        if (this.FeedTitle is not null) {
            result.Append(" (");
            result.Append(this.FeedTitle);
            result.Append(')');
        }

        return result.ToString();
    }
    
    static bool NonEmpty(string? value) => !string.IsNullOrWhiteSpace(value);
}