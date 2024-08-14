using FA;

using TTRSS;

var instanceUrl = new Uri(args[0]);
var rss = new TinyTinyRssConnection(instanceUrl) {
    User = async () => Environment.GetEnvironmentVariable("RSS_USER")
                    ?? throw new ArgumentNullException("RSS_USER"),
    Password = async () => Environment.GetEnvironmentVariable("RSS_PASSWORD")
                        ?? throw new ArgumentNullException("RSS_PASSWORD"),
};

var headlines = await rss.Headlines(CancellationToken.None).ConfigureAwait(false);

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();