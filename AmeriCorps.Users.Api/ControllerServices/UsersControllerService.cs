using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;

namespace AmeriCorps.Users.Api;

public interface IUsersControllerService
{
    Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id);
    Task<(ResponseStatus Status, UserResponse? Response)> GetByExternalAccountId(string externalAccountId);
    Task<(ResponseStatus Status, UserResponse? Response)> CreateOrPatchAsync(UserRequestModel? userRequest);
    Task<(ResponseStatus Status, UserSearchesResponseModel? Response)> GetUserSearchesAsync(int userId);
    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> CreateSearchAsync(int userId, SavedSearchRequestModel? searchRequest);
    Task<(ResponseStatus Status, SavedSearchResponseModel? Response)> UpdateSearchAsync(int userId, int searchId, SavedSearchRequestModel? searchRequest);
    Task<(ResponseStatus Status, bool Response)> DeleteSearchAsync(int userId, int searchId);
    Task<(ResponseStatus Status, CollectionResponseModel? Response)> CreateCollectionAsync(CollectionRequestModel? collectionRequest);

    Task<(ResponseStatus Status,CollectionListResponseModel? Response)> GetCollectionAsync(int userId, string? type);
    
    Task<(ResponseStatus Status,bool Response)> DeleteCollectionAsync(CollectionListRequestModel? collectionListRequestModel);
    Task<(ResponseStatus Status, UserReferencesResponseModel? Response)> GetReferencesAsync(int userId);
    Task<(ResponseStatus Status, ReferenceResponseModel? Response)> CreateReferenceAsync(int userId, ReferenceRequestModel? referenceRequest);
    Task<(ResponseStatus Status, ReferenceResponseModel? Response)> UpdateReferenceAsync(int userId, int referenceId, ReferenceRequestModel? referenceRequest);
    Task<(ResponseStatus Status, bool Response)> DeleteReferenceAsync(int userId, int referenceId);
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
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _respMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> GetByExternalAccountId(string externalAccountId)
    {
        User? user;

        try
        {
            user = await _repository.GetByExternalAcctId(externalAccountId);
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

        var response = _respMapper.Map(user);

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
            Searches = _respMapper.Map(searches)
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
            References = _respMapper.Map(references)
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

        User user = _reqMapper.Map(userRequest);
        User? existingUser = null;

        try
        {
            existingUser = await _repository.GetByExternalAcctId(user.ExternalAccountId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check for user with external account id: {user?.ExternalAccountId}");
            return (ResponseStatus.UnknownError, null);
        }

        try
        {
            if (existingUser == null)
            {   // create user
                user = await _repository.SaveAsync(user);
            }
            else
            {   //patch partial user
                existingUser = PatchExistingUser(existingUser, user);
                user = await _repository.SaveAsync(existingUser);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create user for {userRequest.LastName}, {userRequest.FirstName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(user);

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

        Reference reference = _reqMapper.Map(referenceRequest);
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

        var response = _respMapper.Map(reference);

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

        SavedSearch search = _reqMapper.Map(searchRequest);
        search.UserId = userId;

        try
        {
            search = await _repository.SaveAsync<SavedSearch>(search);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save search {searchRequest?.Name}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(search);

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

        SavedSearch search = _reqMapper.Map(searchRequest);
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

        Reference reference = _reqMapper.Map(referenceRequest);
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

        var response = _respMapper.Map(reference);

        return (ResponseStatus.Successful, response);
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
            await _repository.DeleteAsync<SavedSearch>(searchId);
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

    public async Task<(ResponseStatus Status, CollectionResponseModel? Response)> CreateCollectionAsync(CollectionRequestModel? collectionRequest)
    {
        var  (isValidRequest, response) =  await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, null);
        var collection = _reqMapper.Map(collectionRequest!);

        return await SaveCollectionAsync(collection);

    }

    public async Task<(ResponseStatus Status, CollectionListResponseModel? Response)> GetCollectionAsync(int userId, string? type)
    {
      
        var collectionRequest = new CollectionRequestModel()
        {
            Type = type,
            UserId = userId
        };
      
        var  (isValidRequest, response) =  await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, null);

        var collection = _reqMapper.Map(collectionRequest);

        var userCollections = await _repository.GetCollectionAsync(collection);


        var collectionResponse = _respMapper.Map(userCollections);
        
        return (ResponseStatus.Successful, collectionResponse);

    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteCollectionAsync(
        CollectionListRequestModel? collectionListRequestModel)
    {
        if (collectionListRequestModel == null)
        {
            logger.LogError($"The request is invalid.");
            return (ResponseStatus.MissingInformation, false);
        }

        var collectionRequest = new CollectionRequestModel()
        {
            UserId = collectionListRequestModel.UserId,
            Type = collectionListRequestModel.Type

        };
        var  (isValidRequest, response) =  await IsValidCollectionRequest(collectionRequest);
        if (!isValidRequest)
            return (response, false);
      
        var collection = _reqMapper.Map(collectionListRequestModel);
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
            userExists = await _repository.ExistsAsync<User>(user=> user.Id == userID);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if user {userID} exists.");
            
        }

        return userExists;
    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteReferenceAsync(int userId, int referenceId)
    {

        var references = await _repository.GetUserReferencesAsync(userId);

        if (references == null)
        {
            return (ResponseStatus.MissingInformation, false);
        }
        if (!references.Any(s => s.Id == referenceId))
        {
            _logger.LogInformation($"User with id {userId} does not contain a reference with id {referenceId}.");
            return (ResponseStatus.MissingInformation, false);
        }

        bool deleted = true;
        try
        {
            await _repository.DeleteAsync<Reference>(referenceId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete reference with id {referenceId}.");
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
            search = await _repository.SaveAsync<SavedSearch>(saveRequest);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save search {saveRequest?.Name}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(search);

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

        var response = _respMapper.Map(collection);

        return (ResponseStatus.Successful, response);
    }

    private User PatchExistingUser(User existingUser, User updatedUser)
    {
        existingUser.FirstName = updatedUser.FirstName;
        existingUser.LastName = updatedUser.LastName;
        existingUser.UserName = updatedUser.UserName;
        existingUser.ExternalAccountId = updatedUser.ExternalAccountId;

        //NOTE: this logic assumes that a user only has 1 email address
        var updatedEmail = updatedUser.CommunicationMethods.FirstOrDefault(c => c.Type == "email");

        if (updatedEmail != null)
        {
            existingUser.CommunicationMethods.RemoveAll(c => c.Type == "email");
            existingUser.CommunicationMethods.Add(updatedEmail);
        }

        return existingUser;
    }

    private async Task<(bool,ResponseStatus)> IsValidCollectionRequest(CollectionRequestModel? collectionRequest)
    {
        var isValid = false;
        const ResponseStatus responseStatus = ResponseStatus.Successful;
        if (collectionRequest == null)
            return (isValid,ResponseStatus.MissingInformation);
        
        
        if (!await UserExists(collectionRequest.UserId))
            return (isValid,ResponseStatus.UnknownError);
                
        var validationResponse = validator.Validate(collectionRequest);
        if (!validationResponse?.IsValid ?? false)
        {
            _logger.LogError(validationResponse.ValidationMessage);
            return (isValid,ResponseStatus.UnknownError);
        }
        
        isValid = true;
        return (isValid,responseStatus);
    }
}