using AmeriCorps.Users.Configuration;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Http;

public interface INotificationApiClient
{
    Task<ServiceResponse<UserResponse>> SendInviteEmailAsync(User toInvite);
}

public sealed class NotificationApiClient(
    ILogger<INotificationApiClient> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<NotificationOptions> options)
    : ApiClientBase(logger, httpClientFactory), INotificationApiClient
{
    private readonly NotificationOptions _options = options?.Value ?? new();

    public async Task<ServiceResponse<UserResponse>> SendInviteEmailAsync(User toInvite)
    {
        var uri = new Uri(_options.ApiUrl, $"").ToString();
        var response = await PostAsync<UserResponse>(uri, toInvite);
        return response;
    }
}