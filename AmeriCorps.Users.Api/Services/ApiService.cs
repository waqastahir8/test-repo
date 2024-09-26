using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Services;

public interface IApiService
{
    Task<(bool, UserResponse?)> SendInviteEmailAsync(EmailModel toInvite);
}

public class ApiService(
    INotificationApiClient notificationApiClient
    )
    : ApiServiceBase, IApiService
{
    private readonly INotificationApiClient _notificationApiClient = notificationApiClient;

    public async Task<(bool, UserResponse?)> SendInviteEmailAsync(EmailModel toInvite)
    {
        return await GetContentAsync(async () => await _notificationApiClient.SendInviteEmailAsync(toInvite));
    }
}