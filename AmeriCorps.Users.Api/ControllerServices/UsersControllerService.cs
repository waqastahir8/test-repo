using System.Data;
using AmeriCorps.Users.Api.Services;
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

    Task<(ResponseStatus Status, UserResponse? Response)> AssociateRoleAsync(int userId, int roleId);
    Task<(ResponseStatus Status, UserResponse? Response)> AddRoleToUserAsync(int userId, RoleRequestModel roleRequest);
    Task<(ResponseStatus Status, UserListResponse? Response)> FetchUserListByOrgCodeAsync(String orgCode);
    Task<(ResponseStatus Status, UserResponse? Response)> AddUserToProjectAsync(int userId, string projCode);

    Task<(ResponseStatus Status, UserResponse? Response)> UpdateUserProjectAndRoleDataAsync(UserProjectRoleUpdateRequestModel toUpdate);

    Task<(ResponseStatus Status, UserResponse? Response)> InviteUserToOrgAsync( UserRequestModel toInvite);
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

    private readonly IApiService _apiService;

    private readonly IAccessRepository _accessRepository;
    
    public UsersControllerService(
    ILogger<UsersControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IValidator validator,
    IUserRepository repository,
    IProjectRepository projectRepository,
    IRoleRepository roleRepository,
    IApiService apiService,
    IAccessRepository accessRepository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _validator = validator;
        _repository = repository;
        _projectRepository =  projectRepository;
        _roleRepository = roleRepository;
        _apiService = apiService;
        _accessRepository = accessRepository;
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
                userId = await _repository.GetUserIdByExternalAccountIdAsync(user.ExternalAccountId);
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

        // if (user.Roles.Contains(role))
        // {
        //     return (ResponseStatus.Successful, null);
        // }

        user.Roles.Add( _requestMapper.Map(role));

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

        // if (user.Roles.Contains(role))
        // {
        //     return (ResponseStatus.Successful, null);
        // }

        // user.Roles.Add(role);

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

        UserProject userProject =  new UserProject()
        {
            ProjectName = project.ProjectName,
            ProjectCode = project.ProjectCode,
            ProjectType = project.ProjectType,
            ProjectOrg = project.ProjectOrg,
            Active = true,
            UserId = userId
        };

        if (user.UserProjects.Contains(userProject))
        {
            return (ResponseStatus.Successful, null);
        }else{
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


    public async Task<(ResponseStatus Status, UserListResponse? Response)> FetchUserListByOrgCodeAsync(String orgCode)
    {

        UserList? userList;

        if(orgCode == null || String.IsNullOrEmpty(orgCode))
        {
            return (ResponseStatus.MissingInformation, null);
        }
        

        try
        {
            userList = await  _repository.FetchUserListByOrgCodeAsync(orgCode);
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
        if(toUpdate == null || toUpdate.Id < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        User? existingUser;

        try
        {
            existingUser = await  _repository.GetAsync(toUpdate.Id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve existing user with given user id {Identifier}.", toUpdate.Id.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        User? updatedUser = new User();

        if(existingUser != null)
        {   

            updatedUser = existingUser;

            if(toUpdate.UserRoles != null){
                List<UserRole> userRoleList = new List<UserRole>();
                for (int i = 0; i < toUpdate.UserRoles.Count; i++) 
                {
                    var role = toUpdate.UserRoles[i];
                    // var userRole = await _roleRepository.GetAsync(role.Id);
                    var userRole = await _roleRepository.GetRoleByNameAsync(role.RoleName);

                    if(userRole != null){
                        UserRole uRole = new UserRole()
                        {
                            Id = userRole.Id,
                            RoleName = userRole.RoleName,
                            FunctionalName = userRole.FunctionalName
                        };
                        userRoleList.Add(uRole);
                    }else{
                        _logger.LogError("Could not add user role to user with role name {Identifier}.", role.RoleName.ToString().Replace(Environment.NewLine, ""));
                        return (ResponseStatus.UnknownError, null);
                    }
                }
                updatedUser.Roles = userRoleList;
            }else{
                updatedUser.Roles = existingUser.Roles;
            }

            

            if(toUpdate.UserProjects != null){
                List<UserProject> projectList = new List<UserProject>();
                for (int i = 0; i < toUpdate.UserProjects.Count; i++) 
                {
                    var project = toUpdate.UserProjects[i];
                    var roles = toUpdate.UserProjects[i].ProjectRoles;
                    var accesses = toUpdate.UserProjects[i].ProjectAccess;


                    List<ProjectRole> roleList = new List<ProjectRole>();
                    if(roles != null){
                        for (int j = 0; j < roles.Count; j++) 
                        {
                            var role = roles[i];
                            // var projRole = await _roleRepository.GetAsync(role.Id);
                            var projRole = await _roleRepository.GetRoleByNameAsync(role.RoleName);

                            if(projRole != null){
                                ProjectRole uRole = new ProjectRole()
                                { 
                                    Id = projRole.Id,
                                    RoleName = projRole.RoleName,
                                    FunctionalName = projRole.FunctionalName
                                };
                                roleList.Add(uRole);
                            }else{
                                _logger.LogError("Could not add user role to user with role id {Identifier}.", role.RoleName.ToString().Replace(Environment.NewLine, ""));
                                return (ResponseStatus.UnknownError, null);
                            }
                        }
                    }

                    List<ProjectAccess> accessList = new List<ProjectAccess>();
                    if(accesses != null){
                        for (int j = 0; j < accesses.Count; j++) 
                        {
                            var access = accesses[i];
                            // var projAccess = await _accessRepository.GetAsync(access.Id);
                            var projAccess = await _accessRepository.GetAccessByNameAsync(access.AccessName);

                            if(projAccess != null){
                                ProjectAccess uAccess= new ProjectAccess()
                                {
                                    AccessName = projAccess.AccessName,
                                    AccessLevel = projAccess.AccessLevel
                                };
                                accessList.Add(uAccess);
                            }else{
                                _logger.LogError("Could not add project access to user with access id {Identifier}.", access.AccessName.ToString().Replace(Environment.NewLine, ""));
                                return (ResponseStatus.UnknownError, null);
                            }
                        }
                    }


                    if(!String.IsNullOrEmpty(project.ProjectCode)){ //!String.IsNullOrEmpty(project.Id.ToString())
                        // var orgProj = await _projectRepository.GetAsync(project.Id);
                        var orgProj = await _projectRepository.GetProjectByCodeAsync(project.ProjectCode);
                            if(orgProj != null){
                                UserProject uProject =  new UserProject()
                                {
                                    ProjectName = orgProj.ProjectName,
                                    ProjectCode = orgProj.ProjectCode,
                                    ProjectType = orgProj.ProjectType,
                                    ProjectOrg = orgProj.ProjectOrg,
                                    Active = true,
                                    UserId = toUpdate.Id,
                                    ProjectRoles = roleList,
                                    ProjectAccess = accessList
                                };
                                projectList.Add(uProject);
                            }else{
                                _logger.LogError("Could not add project to user with project id {Identifier}.", project.ProjectCode.ToString().Replace(Environment.NewLine, ""));
                                return (ResponseStatus.UnknownError, null);
                            }
                    } 

                }
                updatedUser.UserProjects = projectList;
            }else{
                updatedUser.UserProjects = existingUser.UserProjects;
            }

            try{
                updatedUser = await _repository.UpdateUserAsync(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save reference for user {updatedUser}.");
                return (ResponseStatus.UnknownError, null);
            }
        }

        var response = _responseMapper.Map(updatedUser);

        return (ResponseStatus.Successful, response);
    }



    public async Task<(ResponseStatus Status, UserResponse? Response)> InviteUserToOrgAsync(UserRequestModel toInvite)
    {

        if(toInvite == null || String.IsNullOrEmpty(toInvite.Email))
        {
            return (ResponseStatus.MissingInformation, null);
        }


        var user = _requestMapper.Map(toInvite);
        if(user != null){
            user.UserName = "InviteHold";

            user = await _repository.SaveAsync(user);
        }else{
             return (ResponseStatus.UnknownError, null);
        }



        if(toInvite.UserProjects != null){
            List<UserProject> projectList = new List<UserProject>();
            for (int i = 0; i < toInvite.UserProjects.Count; i++) 
            {
                var project =  toInvite.UserProjects[i];
                if(!String.IsNullOrEmpty(project.ProjectCode)){
                var orgProj = await _projectRepository.GetProjectByCodeAsync(project.ProjectCode);
                    if(orgProj != null){
                        UserProject uProject =  new UserProject()
                        {
                            ProjectName = orgProj.ProjectName,
                            ProjectCode = orgProj.ProjectCode,
                            ProjectType = orgProj.ProjectType,
                            ProjectOrg = orgProj.ProjectOrg,
                            Active = true,
                            UserId = user.Id
                        };
                        projectList.Add(uProject);
                    }
                }

            }
            user.UserProjects = projectList;
        }

        user = await _repository.SaveAsync(user);

        //Add call for email invite

        // await _apiService.SendInviteEmailAsync(user);

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);

    }

}