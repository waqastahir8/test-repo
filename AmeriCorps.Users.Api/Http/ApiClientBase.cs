using AmeriCorps.Users.Models;
using AmeriCorps.Users.Configuration;

namespace AmeriCorps.Users.Http;

public abstract class ApiClientBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    protected ApiClientBase(ILogger logger, IHttpClientFactory httpClientFactory)
    {
        Logger = logger;
        _httpClientFactory = httpClientFactory;
    }

    protected ILogger Logger { get; }

    public async Task<ServiceResponse<T>> GetAsync<T>(string url) =>
        await CallServiceAsync<T>(url, HttpMethod.Get);

    public async Task<ServiceResponse<T>> DeleteAsync<T>(string url) =>
        await CallServiceAsync<T>(url, HttpMethod.Delete);

    public async Task<ServiceResponse<T>> DeleteAsync<T>(string url, object body) =>
        await CallServiceAsync<T>(url, HttpMethod.Delete, body);

    public async Task<ServiceResponse<T>> UpdateAsync<T>(string url, object body) =>
        await CallServiceAsync<T>(url, HttpMethod.Put, body);

    public async Task<ServiceResponse<T>> PostAsync<T>(string url, object body) =>
        await CallServiceAsync<T>(url, HttpMethod.Post, body);

    public async Task<ServiceResponse<T>> PatchAsync<T>(string url, object body) =>
        await CallServiceAsync<T>(url, HttpMethod.Patch, body);

    private async Task<ServiceResponse<T>> CallServiceAsync<T>(
        string url,
        HttpMethod httpMethod,
        object? body = null)
    {
        var client = _httpClientFactory.CreateClient();

        var httpRequest = new HttpRequestMessage(httpMethod, url);

        if (body is not null)
        {
            httpRequest.Content = JsonContent.Create(body);
        }
        Logger.LogDebug($"Invoking {httpMethod} {url}");

        HttpResponseMessage httpResponse;
        try
        {
            httpResponse = await client.SendAsync(httpRequest);
        }
        catch (Exception e)
        {
            Logger.LogError(e, $"Failed to invoke {httpMethod} {url}");
            throw;
        }

        Logger.LogDebug($"Invoked {httpMethod} {url}. Response: {httpResponse.StatusCode} - {httpResponse.Content}");

        var result = new ServiceResponse<T>
        {
            Successful = httpResponse.IsSuccessStatusCode
        };

        if (result.Successful)
        {
            result.Content = await httpResponse.Content.ReadFromJsonAsync<T>();
        }

        return result;
    }
}