namespace TTRSS;

using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;

public class TinyTinyRss {
    internal readonly HttpClient Client = new();

    public async Task<LoginResponse> Login(LoginRequest request, CancellationToken cancel) {
        var result = await this.Call(request, cancel).ConfigureAwait(false);
        return result.Deserialize<LoginResponse>()!;
    }

    public async Task<JsonNode> Call<TRequest>(TRequest request, CancellationToken cancel)
        where TRequest : RequestBase {
        var response = await this.Client
                                 .PostAsJsonAsync("", request, cancel)
                                 .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Response<JsonNode>>(cancel)
                                   .ConfigureAwait(false);
        return result!.Content;
    }
}