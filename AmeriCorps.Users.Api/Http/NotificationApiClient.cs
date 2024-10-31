using AmeriCorps.Users.Configuration;
using AmeriCorps.Users.Data.Core;
using Microsoft.Extensions.Options;

namespace AmeriCorps.Users.Http;

public interface INotificationApiClient
{
    Task<ServiceResponse<UserResponse>> SendUserInviteEmailAsync(EmailModel email);

    Task<ServiceResponse<OperatingSiteResponse>> SendOperatingSiteInviteEmailAsync(EmailModel email);
}

public sealed class NotificationApiClient(
    ILogger<INotificationApiClient> logger,
    IHttpClientFactory httpClientFactory,
    IOptions<NotificationOptions> options)
    : ApiClientBase(logger, httpClientFactory), INotificationApiClient
{
    private readonly NotificationOptions _options = options?.Value ?? new();

    public async Task<ServiceResponse<UserResponse>> SendUserInviteEmailAsync(EmailModel email)
    {
        var uri = new Uri(_options.ApiUrl, $"/api/Email/send").ToString();
        var response = await PostAsync<UserResponse>(uri, email);
        return response;
    }

    public async Task<ServiceResponse<OperatingSiteResponse>> SendOperatingSiteInviteEmailAsync(EmailModel email)
    {
        var uri = new Uri(_options.ApiUrl, $"/api/Email/send").ToString();
        var response = await PostAsync<OperatingSiteResponse>(uri, email);
        return response;
    }
}