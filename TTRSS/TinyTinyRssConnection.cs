namespace TTRSS;

using System.Text.Json;
using System.Text.Json.Nodes;

public class TinyTinyRssConnection {
    readonly TinyTinyRss ttrss = new();
    string? sessionID;
    public required Func<Task<string>> User { get; set; }
    public required Func<Task<string>> Password { get; set; }

    public Task<Headline[]> Headlines(CancellationToken cancel)
        => this.Authorized(async sid => {
            var headlinesJson = await this.ttrss.Call(new GetHeadlines() {
                FeedID = SpecialFeeds.ALL_ARTICLES,
                Limit = 100,
                SessionID = sid,
                Skip = 0,
                ViewMode = GetHeadlines.ViewModes.ALL_ARTICLES,
            }, cancel).ConfigureAwait(false);
            return headlinesJson.Deserialize<Headline[]>();
        }, cancel);

    async Task<T> Authorized<T>(Func<string, Task<T>> call, CancellationToken cancel) {
        while (true) {
            string sid = InterlockedEx.Read(ref this.sessionID)
                      ?? await this.Authorize(cancel).ConfigureAwait(false);

            try {
                return await call(sid).ConfigureAwait(false);
            } catch (UnauthorizedAccessException) {
                Interlocked.CompareExchange(ref this.sessionID, null, sid);
            }
        }
    }

    async Task<string> Authorize(CancellationToken cancel) {
        string user = await this.User().ConfigureAwait(false);
        string password = await this.Password().ConfigureAwait(false);
        var loginResult = await this.ttrss.Login(new() {
            User = user,
            Password = password,
        }, cancel).ConfigureAwait(false);

        Interlocked.Exchange(ref this.sessionID, loginResult.SessionID);
        return loginResult.SessionID;
    }

    public TinyTinyRssConnection(Uri instance) {
        this.ttrss.Client.BaseAddress = instance
                                     ?? throw new ArgumentNullException(nameof(instance));
    }
}