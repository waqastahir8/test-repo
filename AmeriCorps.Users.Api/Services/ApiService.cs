using AmeriCorps.Users.Http;
using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api.Services;

public interface IApiService
{
    Task<(bool,UserResponse?)> SendInviteEmailAsync(User toInvite);


}


public class ApiService(
    INotificationApiClient notificationApiClient
    )
    : ApiServiceBase, IApiService
{
    private readonly INotificationApiClient _notificationApiClient = notificationApiClient;
    
    public async Task<(bool,UserResponse?)> SendInviteEmailAsync(User toInvite)
    {
        return  await GetContentAsync(async () => await _notificationApiClient.SendInviteEmailAsync(toInvite));

    }


}