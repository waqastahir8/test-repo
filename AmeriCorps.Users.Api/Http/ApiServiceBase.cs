namespace AmeriCorps.Users.Http;

public abstract class ApiServiceBase
{
    protected ApiServiceBase()
    {
    }

    protected static async Task<(bool Success, TContent? Content)> GetContentAsync<T, TContent>(
        Func<Task<ServiceResponse<T>>> apiCallAsync,
        Func<T?, TContent?> getContent)
    {
        try
        {
            var response = await apiCallAsync();
            return (response.Successful, getContent(response.Content));
        }
        catch (Exception)
        {
            return (false, default);
        }
    }

    protected static async Task<(bool Success, T? Content)> GetContentAsync<T>(
        Func<Task<ServiceResponse<T>>> apiCallAsync) =>
        await GetContentAsync(apiCallAsync, x => x);
}