using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api;

public interface IUsersControllerService
{
    Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id);

    Task<(ResponseStatus Status, IEnumerable<UserResponse> Response)> GetAsync(string attributeType, string attributeValue);

    Task<(ResponseStatus Status, UserResponse? Response)> GetByExternalAccountIdAsync(string externalAccountId);

    Task<(ResponseStatus Status, UserResponse? Response)> CreateOrPatchAsync(UserRequestModel? userRequest);

    Task<(ResponseStatus Status, UserSearchesResponseModel? Response)> GetUserSearchesAsync(int userId);

    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> CreateSearchAsync(int userId, SavedSearchRequestModel? searchRequest);

    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> UpdateSearchAsync(int userId, int searchId, SavedSearchRequestModel? searchRequest);

    Task<(ResponseStatus Status, bool Response)> DeleteSearchAsync(int userId, int searchId);

    Task<(ResponseStatus Status, CollectionResponseModel? Response)> CreateCollectionAsync(CollectionRequestModel? collectionRequest);

    Task<(ResponseStatus Status, CollectionListResponseModel? Response)> GetCollectionAsync(int userId, string? type);

    Task<(ResponseStatus Status, bool Response)> DeleteCollectionAsync(CollectionListRequestModel? collectionListRequestModel);

    Task<(ResponseStatus Status, UserReferencesResponseModel? Response)> GetReferencesAsync(int userId);

    Task<(ResponseStatus Status, ReferenceResponseModel? Response)> CreateReferenceAsync(int userId, ReferenceRequestModel? referenceRequest);

    Task<(ResponseStatus Status, ReferenceResponseModel? Response)> UpdateReferenceAsync(int userId, int referenceId, ReferenceRequestModel? referenceRequest);

    Task<(ResponseStatus Status, bool Response)> DeleteReferenceAsync(int userId, int referenceId);
}

public sealed class UsersControllerService : IUsersControllerService
{
    private readonly ILogger _logger;

    private readonly IRequestMapper _requestMapper;

    private readonly IResponseMapper _responseMapper;

    private readonly IValidator _validator;

    private readonly IUserRepository _repository;

    public UsersControllerService(
    ILogger<UsersControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IValidator validator,
    IUserRepository repository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _validator = validator;
        _repository = repository;
    }

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
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, IEnumerable<UserResponse> Response)> GetAsync(
        string attributeType,
        string attributeValue)
    {
        IEnumerable<User>? users;

        if (string.IsNullOrWhiteSpace(attributeType) || string.IsNullOrWhiteSpace(attributeValue))
        {
            return (ResponseStatus.MissingInformation, []);
        }

        try
        {
            users = await _repository.GetByAttributeAsync(attributeType, attributeValue);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve users with attribute {attributeType} = {attributeValue}");
            return (ResponseStatus.UnknownError, []);
        }

        if (users == null)
        {
            return (ResponseStatus.UnknownError, []);
        }

        var response = users.Select(x => _responseMapper.Map(x)!);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> GetByExternalAccountIdAsync(string externalAccountId)
    {
        User? user;

        try
        {
            user = await _repository.GetByExternalAccountIdAsync(externalAccountId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with external account {externalAccountId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserSearchesResponseModel? Response)>
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
            return (ResponseStatus.UnknownError, null);
        }

        if (searches == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = new UserSearchesResponseModel
        {
            UserId = userId,
            Searches = _responseMapper.Map(searches)
        };

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserReferencesResponseModel? Response)>
                                                    GetReferencesAsync(int userId)
    {
        List<Reference>? references;

        try
        {
            references = await _repository.GetUserReferencesAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve references for user with id {userId}.");
            references = null;
        }

        if (references == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = new UserReferencesResponseModel
        {
            UserId = userId,
            References = _responseMapper.Map(references)
        };

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)>
        CreateOrPatchAsync(UserRequestModel? userRequest)
    {
        if (userRequest == null || !_validator.Validate(userRequest))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var user = _requestMapper.Map(userRequest);

        // TODO Fix this, get rid of ExternalAccountId since it is redundant. User username instead.
        int userId = !string.IsNullOrWhiteSpace(user.ExternalAccountId) && int.TryParse(user.ExternalAccountId, out var id)
            ? id
            : 0;
        try
        {
            if (userId == 0 && !string.IsNullOrWhiteSpace(user.ExternalAccountId))
            {
                await _repository.GetUserIdByExternalAccountIdAsync(user.ExternalAccountId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check for user with external account id: {user?.ExternalAccountId}");
            return (ResponseStatus.UnknownError, null);
        }

        try
        {
            if (userId <= 0)
            {
                user.Id = 0;
                user = await _repository.SaveAsync(user);
            }
            else
            {
                user.Id = userId;
                user = await _repository.UpdateUserAsync(user);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create user for {userRequest.LastName}, {userRequest.FirstName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, ReferenceResponseModel? Response)>
            CreateReferenceAsync(int userId, ReferenceRequestModel? referenceRequest)
    {
        bool userExists = false;
        try
        {
            userExists = await _repository.ExistsAsync<User>(u => u.Id == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (referenceRequest == null || !_validator.Validate(referenceRequest) || !userExists)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Reference reference = _requestMapper.Map(referenceRequest);
        reference.UserId = userId;

        try
        {
            reference = await _repository.SaveAsync<Reference>(reference);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save reference for user {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(reference);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, SavedSearchResponseModel? Response)>
              CreateSearchAsync(int userId, SavedSearchRequestModel? searchRequest)
    {
        bool userExists = false;
        try
        {
            userExists = await _repository.ExistsAsync<User>(u => u.Id == userId);
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

        SavedSearch search = _requestMapper.Map(searchRequest);
        search.UserId = userId;

        try
        {
            search = await _repository.SaveAsync<SavedSearch>(search);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save search {searchRequest}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(search);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, SavedSearchResponseModel? Response)>
                UpdateSearchAsync(int userId, int searchId, SavedSearchRequestModel? searchRequest)
    {
        bool userExists = false;

        try
        {
            userExists = await _repository.ExistsAsync<User>(u => u.Id == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        bool searchExists = false;
        try
        {
            searchExists = await _repository.ExistsAsync<SavedSearch>(s => s.UserId == userId &&
                                                                     s.Id == searchId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if search {searchId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (searchRequest == null || !_validator.Validate(searchRequest) || !userExists || !searchExists)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SavedSearch search = _requestMapper.Map(searchRequest);
        search.Id = searchId;
        search.UserId = userId;

        return await SaveSearchAsync(search);
    }

    public async Task<(ResponseStatus Status, ReferenceResponseModel? Response)> UpdateReferenceAsync(int userId, int referenceId, ReferenceRequestModel? referenceRequest)
    {
        bool userExists = false;

        try
        {
            userExists = await _repository.ExistsAsync<User>(u => u.Id == userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        bool referenceExists = false;
        try
        {
            referenceExists = await _repository.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                                     r.Id == referenceId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if search {referenceId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (referenceRequest == null || !_validator.Validate(referenceRequest) || !userExists || !referenceExists)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Reference reference = _requestMapper.Map(referenceRequest);
        reference.Id = referenceId;
        reference.UserId = userId;

        try
        {
            reference = await _repository.SaveAsync<Reference>(reference);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save reference {referenceId}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(reference);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteSearchAsync(int userId, int searchId)
    {
        var searchExists =
            await _repository.ExistsAsync<SavedSearch>(s => s.UserId == userId && s.Id == searchId);

        if (!searchExists)
        {
            _logger.LogInformation($"User with id {userId} does not contain a search with id {searchId}.");
            return (ResponseStatus.MissingInformation, false);
        }

        bool deleted = true;
        try
        {
            deleted = await _repository.DeleteAsync<SavedSearch>(searchId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete search with id {searchId}.");
            return (ResponseStatus.UnknownError, deleted);
        }

        return (ResponseStatus.Successful, deleted);
    }

    public async Task<(ResponseStatus Status, CollectionResponseModel? Response)> CreateCollectionAsync(CollectionRequestModel? collectionRequest)
    {
        var (isValidRequest, response) = await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, null);
        var collection = _requestMapper.Map(collectionRequest!);

        return await SaveCollectionAsync(collection);
    }

    public async Task<(ResponseStatus Status, CollectionListResponseModel? Response)> GetCollectionAsync(int userId, string? type)
    {
        if (type == null)
        {
            _logger.LogInformation("Collection type is null.");
            return (ResponseStatus.MissingInformation, null);
        }

        var collectionRequest = new CollectionRequestModel()
        {
            Type = type,
            UserId = userId
        };

        var (isValidRequest, response) = await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, null);

        var collection = _requestMapper.Map(collectionRequest);

        var userCollections = await _repository.GetCollectionAsync(collection);

        var collectionResponse = _responseMapper.Map(userCollections);

        return (ResponseStatus.Successful, collectionResponse);
    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteCollectionAsync(
        CollectionListRequestModel? collectionListRequestModel)
    {
        if (collectionListRequestModel == null)
        {
            _logger.LogError($"The request is invalid.");
            return (ResponseStatus.MissingInformation, false);
        }

        var collectionRequest = new CollectionRequestModel()
        {
            UserId = collectionListRequestModel.UserId,
            Type = collectionListRequestModel.Type
        };
        var (isValidRequest, response) = await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, false);

        var collection = _requestMapper.Map(collectionListRequestModel);
        var isDeletedSuccessful = await _repository.DeleteCollectionAsync(collection);
        if (!isDeletedSuccessful)
        {
            _logger.LogError("Unable to delete listings from user collections");
            return (ResponseStatus.UnknownError, false);
        }
        return (ResponseStatus.Successful, true);
    }

    private async Task<bool> UserExists(int userID)
    {
        var userExists = false;
        try
        {
            userExists = await _repository.ExistsAsync<User>(user => user.Id == userID);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userID} exists.");
        }

        return userExists;
    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteReferenceAsync(int userId, int referenceId)
    {
        var referenceExists = await _repository.ExistsAsync<Reference>(r => r.UserId == userId &&
                                                   r.Id == referenceId);

        if (!referenceExists)
        {
            _logger.LogInformation($"User with id {userId} does not contain a reference with id {referenceId}.");
            return (ResponseStatus.MissingInformation, false);
        }

        bool deleted = true;
        try
        {
            deleted = await _repository.DeleteAsync<Reference>(referenceId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete reference with id {referenceId}.");
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
            search = await _repository.SaveAsync<SavedSearch>(saveRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save search {saveRequest?.Name}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(search);

        return (ResponseStatus.Successful, response);
    }

    private async Task<(ResponseStatus Status, CollectionResponseModel? Response)>
        SaveCollectionAsync(Collection collectionRequest)
    {
        Collection? collection;
        try
        {
            collection = await _repository.SaveAsync(collectionRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save collection {collectionRequest?.ListingId}.");
            collection = null;
        }

        if (collection == null)
        {
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(collection);

        return (ResponseStatus.Successful, response);
    }

    private async Task<(bool, ResponseStatus)> IsValidCollectionRequest(CollectionRequestModel? collectionRequest)
    {
        var isValid = false;
        const ResponseStatus responseStatus = ResponseStatus.Successful;
        if (collectionRequest == null)
            return (isValid, ResponseStatus.MissingInformation);

        if (!await UserExists(collectionRequest.UserId))
            return (isValid, ResponseStatus.UnknownError);

        var validationResponse = _validator.Validate(collectionRequest);
        if (!validationResponse?.IsValid ?? false)
        {
            _logger.LogError(validationResponse?.ValidationMessage);
            return (isValid, ResponseStatus.UnknownError);
        }

        isValid = true;
        return (isValid, responseStatus);
    }
}