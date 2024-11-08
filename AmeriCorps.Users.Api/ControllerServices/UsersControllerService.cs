using System.Data;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data.Core.Model;
using Microsoft.EntityFrameworkCore.Storage;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    Task<(ResponseStatus Status, UserResponse? Response)> AssociateRoleAsync(int userId, int roleId);

    Task<(ResponseStatus Status, UserResponse? Response)> AddRoleToUserAsync(int userId, RoleRequestModel roleRequest);

    Task<(ResponseStatus Status, UserListResponse? Response)> FetchUserListByOrgCodeAsync(String orgCode);

    Task<(ResponseStatus Status, UserResponse? Response)> AddUserToProjectAsync(int userId, string projCode);

    Task<(ResponseStatus Status, UserResponse? Response)> UpdateUserProjectAndRoleDataAsync(UserProjectRoleUpdateRequestModel toUpdate);

    Task<(ResponseStatus Status, UserResponse? Response)> InviteUserAsync(UserRequestModel toInvite);

    Task<(ResponseStatus Status, UserResponse? Response)> LinkNewAccountToExistingUserAsync(ExistingUserSearchModel toLink);

    Task<(ResponseStatus Status, DirectDepositResponse? Response)> SaveDirectDepositFormAsync(int userId, DirectDepositRequestModel? toUpdate);

    Task<(ResponseStatus Status, bool Response)> DeleteDirectDepositFormAsync(int userId, int directDepositId);
    Task<(ResponseStatus Status, TaxWithHoldingResponse? Response)> SaveTaxWithholdingFormAsync(int userId, TaxWithHoldingRequestModel? toUpdate);

    Task<(ResponseStatus Status, bool Response)> DeleteTaxWithholdingFormAsync(int userId, int taxWithHoldingId);

    Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfo(int userId, SocialSecurityVerificationRequestModel verificationUpdate);

}

public sealed class UsersControllerService : IUsersControllerService
{
    private readonly ILogger _logger;

    private readonly IRequestMapper _requestMapper;

    private readonly IResponseMapper _responseMapper;

    private readonly IValidator _validator;

    private readonly IUserRepository _repository;

    private readonly IProjectRepository _projectRepository;

    private readonly IRoleRepository _roleRepository;

    private readonly IUserHelperService _userHelperService;

    private readonly IAccessRepository _accessRepository;

    private readonly INotificationApiClient _notificationApiClient; 

    private readonly IEncryptionService _encryptionService;


    public UsersControllerService(
        ILogger<UsersControllerService> logger,
        IRequestMapper requestMapper,
        IResponseMapper responseMapper,
        IValidator validator,
        IUserRepository repository,
        IProjectRepository projectRepository,
        IRoleRepository roleRepository,
        IAccessRepository accessRepository,
        IUserHelperService userHelperService,
        INotificationApiClient notificationApiClient,
        IEncryptionService encryptionService)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _validator = validator;
        _repository = repository;
        _projectRepository = projectRepository;
        _roleRepository = roleRepository;
        _accessRepository = accessRepository;
        _userHelperService = userHelperService;
        _notificationApiClient = notificationApiClient;
        _encryptionService = encryptionService;
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

        if (!string.IsNullOrWhiteSpace(user.EncryptedSocialSecurityNumber))
        {
            user.EncryptedSocialSecurityNumber = _encryptionService.Decrypt(user.EncryptedSocialSecurityNumber);
        }
        user.TaxWithHoldings = user.TaxWithHoldings.OrderByDescending(t => t.ModifiedDate).ToList();

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
            _logger.LogError(e, "Could not retrieve users with attribute {AttributeType} = {AttributeValue}", attributeType.Replace(Environment.NewLine, ""), attributeValue.Replace(Environment.NewLine, ""));
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
            _logger.LogError(e, "Could not retrieve user with external account {ExternalAccountId}.", externalAccountId.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.NotFound, null);
        }

        if (!string.IsNullOrWhiteSpace(user.EncryptedSocialSecurityNumber))
        {
            user.EncryptedSocialSecurityNumber = _encryptionService.Decrypt(user.EncryptedSocialSecurityNumber);
        }

        user.TaxWithHoldings = user.TaxWithHoldings.OrderByDescending(t => t.ModifiedDate).ToList();

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

        if (!string.IsNullOrWhiteSpace(userRequest.EncryptedSocialSecurityNumber))
        {
            userRequest.EncryptedSocialSecurityNumber = _encryptionService.Encrypt(userRequest.EncryptedSocialSecurityNumber);
        }

        var user = _requestMapper.Map(userRequest);

        // TODO Fix this, get rid of ExternalAccountId since it is redundant. Use username instead.
        int userId = !string.IsNullOrWhiteSpace(user.ExternalAccountId) && int.TryParse(user.ExternalAccountId, out var id)
            ? id
            : 0;
        try
        {
            if (userId == 0 && !string.IsNullOrWhiteSpace(user.ExternalAccountId))
            {
                userId = await _repository.GetUserIdByExternalAccountIdAsync(user.ExternalAccountId);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to check for user with external account id: {User}",
                                        user.ExternalAccountId.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        user.UpdatedDate = DateTime.UtcNow;

        try
        {
            if (userId <= 0)
            {
                user.Id = 0;
                user.UserAccountStatus = UserAccountStatus.ACTIVE;
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
            _logger.LogError(e, "Unable to create user for {LastName}, {FirstName}.", userRequest.LastName.Replace(Environment.NewLine, ""), userRequest.FirstName.Replace(Environment.NewLine, ""));
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
            _logger.LogError(e, "Unable to save search with exception: {ExceptionMessage}.", e.Message);
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
        {
            return (response, null);
        }
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
        {
            return (response, null);
        }

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
        {
            return (response, false);
        }

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
        var referenceExists = await _repository.ExistsAsync<Reference>(r => r.UserId == userId && r.Id == referenceId);

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
            _logger.LogError(e, "Unable to save search {SaveReuest}", saveRequest?.Name.Replace(Environment.NewLine, "") ?? "");
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

    public async Task<(ResponseStatus Status, UserResponse? Response)> AssociateRoleAsync(int userId, int roleId)
    {
        User? user;
        try
        {
            user = await _repository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Role? role;

        try
        {
            role = await _repository.GetRoleAsync(roleId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve role with id {roleId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (role == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        user.Roles.Add(_requestMapper.Map(role));

        try
        {
            await _repository.UpdateUserAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }
        return (ResponseStatus.Successful, _responseMapper.Map(user));
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> AddRoleToUserAsync(int userId, RoleRequestModel roleRequest)
    {
        User? user;
        try
        {
            user = await _repository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }
        Role? role;
        role = _requestMapper.Map(roleRequest);

        if (role == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        try
        {
            await _repository.UpdateUserAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        return (ResponseStatus.Successful, _responseMapper.Map(user));
    }

    public async Task<(ResponseStatus Status, RoleResponse? Response)> GetRoleAsync(int id)
    {
        Role? role;

        try
        {
            role = await _repository.GetRoleAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve role with id {id}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (role == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(role);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> AddUserToProjectAsync(int userId, string projCode)
    {
        User? user;
        try
        {
            user = await _repository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Project? project;

        try
        {
            project = await _projectRepository.GetProjectByCodeAsync(projCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve project with code {projCode}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (project == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        UserProject userProject = new UserProject()
        {
            ProjectName = project.ProjectName,
            ProjectCode = project.ProjectCode,
            ProjectType = project.ProjectType,
            ProjectOrg = project.ProjectOrgCode,
            Active = true,
            UserId = userId
        };

        if (user.UserProjects.Contains(userProject))
        {
            return (ResponseStatus.Successful, null);
        }
        else
        {
            user.UserProjects.Add(userProject);
        }

        try
        {
            await _repository.UpdateUserAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }
        return (ResponseStatus.Successful, _responseMapper.Map(user));
    }

    public async Task<(ResponseStatus Status, UserListResponse? Response)> FetchUserListByOrgCodeAsync(string orgCode)
    {
        UserList? userList;

        if (string.IsNullOrWhiteSpace(orgCode))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        try
        {
            userList = await _repository.FetchUserListByOrgCodeAsync(orgCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve userList with given orgName {orgCode}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(userList);
        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> UpdateUserProjectAndRoleDataAsync(UserProjectRoleUpdateRequestModel toUpdate)
    {
        if (toUpdate == null || toUpdate.Id < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        User? existingUser;

        try
        {
            existingUser = await _repository.GetAsync(toUpdate.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve existing user with given user id {Identifier}.", toUpdate.Id.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        User? updatedUser = new User();

        if (existingUser != null)
        {
            updatedUser = existingUser;

            await UpdateUserAccountStatusAsync(updatedUser, (UserAccountStatus)toUpdate.UserAccountStatus);

            updatedUser.Roles = await UpdateUserRolesAsync(toUpdate.UserRoles);

            updatedUser.UserProjects = await UpdateUserProjectsAsync(toUpdate.UserProjects);

            if (updatedUser.Roles.Count != toUpdate.UserRoles.Count || updatedUser.UserProjects.Count != toUpdate.UserProjects.Count)
            {
                _logger.LogError("Unable to update Roles or Project for update {Identifier}.", toUpdate.Id.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.UnknownError, null);
            }

            updatedUser.UpdatedDate = DateTime.UtcNow;

            try
            {
                updatedUser = await _repository.UpdateUserAsync(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to save reference for user {Identifier}.", toUpdate.Id.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.UnknownError, null);
            }
        }

        var response = _responseMapper.Map(updatedUser);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> InviteUserAsync(UserRequestModel toInvite)
    {
        if (toInvite == null || String.IsNullOrEmpty(toInvite.Email))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var user = _requestMapper.Map(toInvite);

        if (user != null)
        {
            user.UserAccountStatus = UserAccountStatus.INVITED;

            user.UpdatedDate = DateTime.UtcNow;

            user.InviteDate = DateTime.UtcNow;
        }
        else
        {
            return (ResponseStatus.UnknownError, null);
        }

        user.Roles = await UpdateUserRolesAsync(toInvite.UserRoles);

        user.UserProjects = await UpdateUserProjectsAsync(toInvite.UserProjects);

        CommunicationMethod email = new CommunicationMethod()
        {
            Type = "email",
            Value = toInvite.Email,
            IsPreferred = true
        };

        List<CommunicationMethod> commList = new List<CommunicationMethod>();
        commList.Add(email);
        user.CommunicationMethods = commList;
        user.UserName = toInvite.Email;

        try
        {
            user = await _repository.SaveAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to save for user {Identifier} for invite.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var success = false;

        try
        {
            success = await _userHelperService.SendUserInviteAsync(user);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send invite for user {Identifier}.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (!success)
        {
            _logger.LogInformation("Invite email not sent for {Identifier}.", toInvite.UserName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> LinkNewAccountToExistingUserAsync(ExistingUserSearchModel toLink)
    {
        if (toLink == null || String.IsNullOrEmpty(toLink.UserEmail) || toLink.NewUser == null || String.IsNullOrEmpty(toLink.NewUser.OrgCode))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        User? existingUser;

        try
        {
            existingUser = await _repository.FindInvitedUserInfo(toLink.UserEmail, toLink.NewUser.OrgCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve existing user with given user id {Identifier}.", toLink.UserEmail.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (existingUser != null)
        {
            existingUser.FirstName = toLink.NewUser.FirstName;
            existingUser.LastName = toLink.NewUser.LastName;
            existingUser.UserName = toLink.NewUser.UserName;
            existingUser.PreferredName = toLink.NewUser.PreferredName;

            await UpdateUserAccountStatusAsync(existingUser, UserAccountStatus.ACTIVE);

            existingUser.UpdatedDate = DateTime.UtcNow;

            try
            {
                await _repository.SaveAsync(existingUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unable to save for user {Identifier} for invite.", existingUser.UserName.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.UnknownError, null);
            }
        }

        var response = _responseMapper.Map(existingUser);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, DirectDepositResponse? Response)> SaveDirectDepositFormAsync(int userId, DirectDepositRequestModel? toUpdate)
    {
        if (toUpdate == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }
        User? existingUser;
        try
        {
            existingUser = await _repository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve existing user with given user id {Identifier}.", userId.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        DirectDeposit directDeposit = _requestMapper.Map(toUpdate);
        directDeposit.UserId = userId;
        try
        {
            var deleted = true;
            if (existingUser?.DirectDeposits.Count > 0)
            {
                deleted = DeleteDirectDepositFormAsync(userId, existingUser.DirectDeposits[0].Id).Result.Response;
            }
            directDeposit = await _repository.SaveAsync<DirectDeposit>(directDeposit);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save direct deposit for user {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(directDeposit);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, bool Response)> DeleteDirectDepositFormAsync(int userId, int directDepositId)
    {
        var directDepositExists = await _repository.ExistsAsync<DirectDeposit>(d => d.UserId == userId && d.Id == directDepositId);
        if (!directDepositExists)
        {
            _logger.LogInformation($"User with id {userId} does not contain a direct deposit with id {directDepositId}.");
            return (ResponseStatus.MissingInformation, false);
        }
        bool deleted = true;
        try
        {
            deleted = await _repository.DeleteAsync<DirectDeposit>(directDepositId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete direct deposit with id {directDepositId}.");
            return (ResponseStatus.UnknownError, deleted);
        }
        return (ResponseStatus.Successful, deleted);
    }

    public async Task<(ResponseStatus Status, TaxWithHoldingResponse? Response)> SaveTaxWithholdingFormAsync(int userId, TaxWithHoldingRequestModel? taxWithHoldingRequestModel)
    {
        if (taxWithHoldingRequestModel == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }
        User? existingUser;
        try
        {
            existingUser = await _repository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve existing user with given user id {Identifier}.", userId.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        TaxWithHolding taxWithHolding = _requestMapper.Map(taxWithHoldingRequestModel);
        taxWithHolding.UserId = userId;
        List<TaxWithHolding>? taxWithholdings;
        taxWithHolding.ModifiedDate = DateTime.UtcNow;

        taxWithHolding = await _repository.SaveAsync<TaxWithHolding>(taxWithHolding);

        var response = _responseMapper.Map(taxWithHolding);

        List<string> recipients = new List<string>();

        if (existingUser?.CommunicationMethods != null)
        {
            for (int i = 0; i < existingUser.CommunicationMethods.Count; i++)
            {
                if (existingUser.CommunicationMethods[i].Type == "email" && existingUser.CommunicationMethods[i].IsPreferred)
                {
                    recipients.Add(existingUser.CommunicationMethods[i].Value);
                }
            }
        }

        if (recipients.Count > 0)
        {
            var emailModel = new EmailModel
            {
                Recipients = recipients,
                Subject = "Tax Withholding Form Saved",
                Content = "Your tax withholding form has been successfully saved."
            };

            try
            {
                var emailResponse = await _notificationApiClient.SendUserInviteEmailAsync(emailModel);
                if (!emailResponse.Successful)
                {
                    _logger.LogWarning("Failed to send tax withholding email notification to user {Identifier}.", userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while sending tax withholding email notification to user {Identifier}.", userId);
            }
        }
        else
        {
            _logger.LogWarning("No preferred email found for user {Identifier}. Email notification was not sent.", userId);
        }

        return (ResponseStatus.Successful, response);
    }


    public async Task<(ResponseStatus Status, bool Response)> DeleteTaxWithholdingFormAsync(int userId, int taxWithHoldingId)
    {
        var taxWithHoldingExists = await _repository.ExistsAsync<TaxWithHolding>(t => t.UserId == userId && t.Id == taxWithHoldingId);
        if (!taxWithHoldingExists)
        {
            _logger.LogInformation($"User with id {userId} does not contain a tax withholding with id {taxWithHoldingId}.");
            return (ResponseStatus.MissingInformation, false);
        }
        bool deleted = true;
        try
        {
            deleted = await _repository.DeleteAsync<TaxWithHolding>(taxWithHoldingId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to delete tax withholding with id {taxWithHoldingId}.");
            return (ResponseStatus.UnknownError, deleted);
        }
        return (ResponseStatus.Successful, deleted);
    }

    public async Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfo(int userId, SocialSecurityVerificationRequestModel verificationUpdate)
    {
        if (verificationUpdate == null || userId < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SocialSecurityVerification? userStatus;

        try
        {
            userStatus = await _repository.FindSocialSecurityVerificationByUserId(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if verification for {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if(userStatus != null){
            userStatus.LastSubmitUser = verificationUpdate.LastSubmitUser;
            if(userStatus.CitizenshipStatus != (VerificationStatus)verificationUpdate.CitizenshipStatus)
            {
                userStatus.CitizenshipStatus = (VerificationStatus)verificationUpdate.CitizenshipStatus;
                userStatus.CitizenshipUpdatedDate = DateTime.UtcNow;
            }
            if(userStatus.SocialSecurityStatus != (VerificationStatus)verificationUpdate.SocialSecurityStatus)
            {
                userStatus.SocialSecurityStatus = (VerificationStatus)verificationUpdate.SocialSecurityStatus;
                userStatus.SocialSecurityUpdatedDate = DateTime.UtcNow;
            }

            if((userStatus.SocialSecurityStatus == VerificationStatus.Resubmit || userStatus.CitizenshipStatus == VerificationStatus.Resubmit)&& userStatus.SubmitCount < 5){
                //ReSubmit Package
                userStatus.SubmitCount++;
            }

            try
            {
                // userStatus = await _repository.SaveSSAInfo(userStatus);
                userStatus = await _repository.SaveAsync(userStatus);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save update for user {userId} exists.");
                return (ResponseStatus.UnknownError, null);
            }
        }

        var response = _responseMapper.Map(userStatus);

        return (ResponseStatus.Successful, response);
    }

    private async Task UpdateUserAccountStatusAsync(User updatedUser, UserAccountStatus newStatus)
    {
        switch (newStatus)
        {
            case UserAccountStatus.PENDING:
                var sent = await _userHelperService.ResendUserInviteAsync(updatedUser);
                if (sent)
                {
                    updatedUser.UserAccountStatus = UserAccountStatus.PENDING;
                }
                break;

            case UserAccountStatus.INVITED:
                updatedUser.UserAccountStatus = UserAccountStatus.INVITED;
                break;

            case UserAccountStatus.ACTIVE:
                updatedUser.UserAccountStatus = UserAccountStatus.ACTIVE;
                break;

            case UserAccountStatus.DEACTIVE:
                updatedUser.UserAccountStatus = UserAccountStatus.DEACTIVE;
                break;

            default:
                break;
        }
    }

    private async Task<List<UserRole>> UpdateUserRolesAsync(List<UserRoleRequestModel> updatedRoles)
    {
        List<UserRole> userRoleList = new List<UserRole>();
        if (updatedRoles != null && updatedRoles.Count > 0)
        {
            for (int i = 0; i < updatedRoles.Count; i++)
            {
                var role = updatedRoles[i];
                var userRole = await _roleRepository.GetRoleByNameAsync(role.RoleName);

                if (userRole != null)
                {
                    UserRole uRole = new UserRole()
                    {
                        RoleName = userRole.RoleName,
                        FunctionalName = userRole.FunctionalName
                    };
                    userRoleList.Add(uRole);
                }
                else
                {
                    _logger.LogError("Could not add user role to user with role name {Identifier}.", role.RoleName.ToString().Replace(Environment.NewLine, ""));
                }
            }
        }
        return userRoleList;
    }

    private async Task<List<UserProject>> UpdateUserProjectsAsync(List<UserProjectRequestModel> updatedProjects)
    {
        List<UserProject> projectList = new List<UserProject>();
        if (updatedProjects != null && updatedProjects.Count > 0)
        {
            for (int i = 0; i < updatedProjects.Count; i++)
            {
                var project = updatedProjects[i];
                var roles = updatedProjects[i].ProjectRoles;
                var accesses = updatedProjects[i].ProjectAccess;

                List<ProjectRole> roleList = new List<ProjectRole>();
                if (roles != null)
                {
                    for (int j = 0; j < roles.Count; j++)
                    {
                        var role = roles[j];
                        var projRole = await _roleRepository.GetRoleByNameAsync(role.RoleName);

                        if (projRole != null)
                        {
                            ProjectRole uRole = new ProjectRole()
                            {
                                RoleName = projRole.RoleName,
                                FunctionalName = projRole.FunctionalName
                            };
                            roleList.Add(uRole);
                        }
                        else
                        {
                            _logger.LogError("Could not add user role to user with role id {Identifier}.", role.RoleName.ToString().Replace(Environment.NewLine, ""));
                        }
                    }
                }

                List<ProjectAccess> accessList = new List<ProjectAccess>();
                if (accesses != null)
                {
                    for (int k = 0; k < accesses.Count; k++)
                    {
                        var access = accesses[k];
                        var projAccess = await _accessRepository.GetAccessByNameAsync(access.AccessName);

                        if (projAccess != null)
                        {
                            ProjectAccess uAccess = new ProjectAccess()
                            {
                                AccessName = projAccess.AccessName,
                                AccessLevel = projAccess.AccessLevel
                            };
                            accessList.Add(uAccess);
                        }
                        else
                        {
                            _logger.LogError("Could not add project access to user with access id {Identifier}.", access.AccessName.ToString().Replace(Environment.NewLine, ""));
                        }
                    }
                }

                if (!string.IsNullOrEmpty(project.ProjectCode))
                {
                    var orgProj = await _projectRepository.GetProjectByCodeAsync(project.ProjectCode);
                    if (orgProj != null)
                    {
                        UserProject uProject = new UserProject()
                        {
                            ProjectName = orgProj.ProjectName,
                            ProjectCode = orgProj.ProjectCode,
                            ProjectType = orgProj.ProjectType,
                            ProjectOrg = orgProj.ProjectOrgCode,
                            Active = true,
                            ProjectRoles = roleList,
                            ProjectAccess = accessList
                        };
                        projectList.Add(uProject);
                    }
                    else
                    {
                        _logger.LogError("Could not add project to user with project id {Identifier}.", project.ProjectCode.ToString().Replace(Environment.NewLine, ""));
                    }
                }
            }
        }
        return projectList;
    }
}