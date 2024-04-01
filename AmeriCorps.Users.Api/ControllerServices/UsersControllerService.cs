using FluentValidation;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;


namespace AmeriCorps.Users.Api;

public interface IUsersControllerService
{
    Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id);
    Task<(ResponseStatus Status, UserResponse? Response)> CreateAsync(UserRequestModel? userRequest);
    Task<(ResponseStatus Status, UserSearchListResponseModel? Response)> GetUserSearchesAsync(int userId);
    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> CreateSearchAsync(int userId, SavedSearchRequestModel? searchRequest);
    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> UpdateSearchAsync(int userId, int searchId, SavedSearchRequestModel? searchRequest);
    Task<(ResponseStatus Status, bool Response)> DeleteSearchAsync(int userId, int searchId);
}
public sealed class UsersControllerService(
    ILogger<UsersControllerService> logger,
    IRequestMapper reqMapper,
    IResponseMapper respMapper,
    IValidator validator,
    IUserRepository repository)
    : IUsersControllerService
{
    private readonly ILogger<UsersControllerService> _logger = logger;

    private readonly IRequestMapper _reqMapper = reqMapper;
    private readonly IResponseMapper _respMapper = respMapper;
    private readonly IValidator _validator = validator;
    private readonly IUserRepository _repository = repository;
    public async Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id)
    {
        User? user;

        try
        {
            user = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with id {id}.");
            user = null;
        }

        if (user == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserSearchListResponseModel? Response)>
                                                        GetUserSearchesAsync(int userId)
    {

        List<SavedSearch>? searches;

        try
        {
            searches = await _repository.GetUserSearchesAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve searches for user with id {userId}.");
            searches = null;
        }

        if (searches == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = new UserSearchListResponseModel
        {
            UserId = userId,
            Searches = _respMapper.Map(searches)
        };

        return (ResponseStatus.Successful, response);


    }

    public async Task<(ResponseStatus Status, UserResponse? Response)>
                                                    CreateAsync(UserRequestModel? userRequest)
    {
        if (userRequest == null || !_validator.Validate(userRequest))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        User? user = _reqMapper.Map(userRequest);

        try
        {
            user = await _repository.CreateAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create user for {userRequest.LastName}, {userRequest.FirstName}.");
            user = null;
        }

        if (user == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, SavedSearchResponseModel? Response)>
              CreateSearchAsync(int userId, SavedSearchRequestModel? searchRequest)
    {
        bool userExists = false;
        try
        {
            userExists = await _repository.ExistsAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (searchRequest == null || !_validator.Validate(searchRequest) || !userExists)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SavedSearch search = _reqMapper.Map(searchRequest);
        search.UserId = userId;
        return await SaveSearchAsync(search);
    }

    public async Task<(ResponseStatus Status, SavedSearchResponseModel? Response)>
                UpdateSearchAsync(int userId, int searchId, SavedSearchRequestModel? searchRequest)
    {
        bool userExists = false;
        try
        {
            userExists = await _repository.ExistsAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (searchRequest == null || !_validator.Validate(searchRequest) || !userExists)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SavedSearch search = _reqMapper.Map(searchRequest);
        search.Id = searchId;
        search.UserId = userId;

        return await SaveSearchAsync(search);

    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteSearchAsync(int userId, int searchId)
    {

        var searches = await _repository.GetUserSearchesAsync(userId);

        if (searches == null)
        {
            return (ResponseStatus.MissingInformation, false);
        }
        if (!searches.Any(s => s.Id == searchId))
        {
            _logger.LogInformation($"User with id {userId} does not contain a search with id {searchId}.");
            return (ResponseStatus.MissingInformation, false);
        }

        bool deleted = true;
        try
        {
            await _repository.DeleteSearchAsync(searchId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete search with id {searchId}.");
            deleted = false;
        }

        if (!deleted)
        {
            return (ResponseStatus.UnknownError, deleted);
        }

        return (ResponseStatus.Successful, deleted);
    }

    private async Task<(ResponseStatus Status, SavedSearchResponseModel? Response)>
              SaveSearchAsync(SavedSearch saveRequest)
    {

        SavedSearch? search;
        try
        {
            search = await _repository.SaveAsync(saveRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save search {saveRequest?.Name}.");
            search = null;
        }

        if (search == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(search);

        return (ResponseStatus.Successful, response);
    }
}