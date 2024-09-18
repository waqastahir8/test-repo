﻿using System.Configuration;
using System.Data;
using System.Security.Cryptography;
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

    Task<(ResponseStatus Status, UserResponse? Response)> UpdateUserDataAsync(UserResponse toUpdate);

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

    private readonly IConfiguration _configuration;


    public UsersControllerService(
    ILogger<UsersControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IValidator validator,
    IUserRepository repository,
    IProjectRepository projectRepository,
    IRoleRepository roleRepository,
    IApiService apiService,
    IConfiguration configuration)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _validator = validator;
        _repository = repository;
        _projectRepository =  projectRepository;
        _roleRepository = roleRepository;
        _apiService = apiService;
        _configuration = configuration;
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
            user.EncryptedSocialSecurityNumber = Decrypt(user.EncryptedSocialSecurityNumber);
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
            _logger.LogError(e, "Could not retrieve users with attribute {AttributeType} = {AttributeValue}",attributeType.Replace(Environment.NewLine, ""),attributeValue.Replace(Environment.NewLine, ""));
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
            _logger.LogError(e, "Could not retrieve user with external account {ExternalAccountId}.",externalAccountId.Replace(Environment.NewLine, ""));
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

        if (!string.IsNullOrWhiteSpace(userRequest.EncryptedSocialSecurityNumber))
        {
            userRequest.EncryptedSocialSecurityNumber = Encrypt(userRequest.EncryptedSocialSecurityNumber);
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
            _logger.LogError(e, "Unable to create user for {LastName}, {FirstName}.",userRequest.LastName.Replace(Environment.NewLine, ""),userRequest.FirstName.Replace(Environment.NewLine, ""));
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
            
            _logger.LogError(e, "Unable to save search with exception: {ExceptionMessage}.",e.Message);
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
            _logger.LogError(e, "Unable to save search {SaveReuest}",saveRequest?.Name.Replace(Environment.NewLine, "") ?? "");
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

        if (user.Roles.Contains(role))
        {
            return (ResponseStatus.Successful, null);
        }

        user.Roles.Add(role);

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

        if (user.Roles.Contains(role))
        {
            return (ResponseStatus.Successful, null);
        }

        user.Roles.Add(role);

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

        if(orgCode == null)
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

    public async Task<(ResponseStatus Status, UserResponse? Response)> UpdateUserDataAsync(UserResponse toUpdate)
    {
        if(toUpdate == null || String.IsNullOrEmpty(toUpdate.Id.ToString()))
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
            _logger.LogError(e, $"Could not retrieve existing user with given user name {toUpdate.UserName}.");
            return (ResponseStatus.UnknownError, null);
        }

        User? updatedUser = new User();

        if(existingUser != null)
        {   
            updatedUser.Id = existingUser.Id;

            updatedUser = existingUser;


            //TODO add access

            if(toUpdate.UserProjects != null){
                List<UserProject> projectList = new List<UserProject>();
                for (int i = 0; i < toUpdate.UserProjects.Count; i++) 
                {
                    var project =  toUpdate.UserProjects[i];
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
                                UserId = toUpdate.Id
                            };
                            projectList.Add(uProject);
                        }
                    }

                }
                updatedUser.UserProjects = projectList;
            }else{
                updatedUser.UserProjects = existingUser.UserProjects;
            }

            if(toUpdate.Roles != null){
                List<Role> roleList = new List<Role>();
                for (int i = 0; i < toUpdate.Roles.Count; i++) 
                {
                    var role = toUpdate.Roles[i];
                    if(!String.IsNullOrEmpty(role.RoleName.ToString())){
                        var orgRole = await _roleRepository.GetRoleByNameAsync(role.RoleName);
                        if(orgRole != null){
                            roleList.Add(orgRole);
                        }
                    }
                }

                updatedUser.Roles = roleList;    

            }else{
                updatedUser.Roles = existingUser.Roles;
            }

            try{
                updatedUser = await _repository.UpdateUserAsync(updatedUser);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save reference for user {updatedUser}.");
                return (ResponseStatus.UnknownError, null);
            }

            if(toUpdate.Roles != null){
                //clear existingUser roles
                for (int i = 0; i < toUpdate.Roles.Count; i++) 
                {
                    var role = toUpdate.Roles[i];
                    if(!String.IsNullOrEmpty(role.RoleName.ToString())){
                        var orgRole = _responseMapper.Map(await _roleRepository.GetRoleByNameAsync(role.RoleName));
                        if(orgRole != null){
                            await AddRoleToUserAsync(toUpdate.Id, orgRole);
                        }
                    }
                }
            }
        }

        var response = _responseMapper.Map(updatedUser);

        return (ResponseStatus.Successful, response);
    }



    public async Task<(ResponseStatus Status, UserResponse? Response)> InviteUserToOrgAsync(UserRequestModel toInvite)
    {

        if(toInvite == null || String.IsNullOrEmpty(toInvite.Email.ToString()))
        {
            return (ResponseStatus.MissingInformation, null);
        }


        var user = _requestMapper.Map(toInvite);
        user.UserName = "InviteHold";

        user = await _repository.SaveAsync(user);


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


        if(toInvite.Roles != null){
            List<Role> roleList = new List<Role>();
            for (int i = 0; i < toInvite.Roles.Count; i++) 
            {
                var role = toInvite.Roles[i];
                if(!String.IsNullOrEmpty(role.RoleName.ToString())){
                    var orgResponse = await _roleRepository.GetRoleByNameAsync(role.RoleName);
                    var orgRole = _responseMapper.Map(orgResponse);
                    if(orgRole != null){
                        await AddRoleToUserAsync(user.Id, orgRole);
                    }
                }
            }
            user.Roles = roleList;    
        }

        user = await _repository.SaveAsync(user);

        //Add call for email invite

        // await _apiService.SendInviteEmailAsync(user);

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);

    }

    private string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("69PhJU1v1SMbE6mRBWalOIQlBqAmvHQ5WCMX4IoCwZ0=");
            aes.IV = Convert.FromBase64String("vNWAOAbK+6wi0NDXbCAncA==");

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(plainText);
                    }
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = Convert.FromBase64String("69PhJU1v1SMbE6mRBWalOIQlBqAmvHQ5WCMX4IoCwZ0=");
            aes.IV = Convert.FromBase64String("vNWAOAbK+6wi0NDXbCAncA==");

            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }


}
