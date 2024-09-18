using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;


namespace AmeriCorps.Users.Api;

public interface IAccessControllerService
{

    Task<(ResponseStatus Status, AccessResponse? Response)> GetAccessByNameAsync(string accessName);

    Task<(ResponseStatus Status, List<AccessResponse>? Response)> GetAccessListAsync();

    Task<(ResponseStatus Status, List<AccessResponse>? Response)> GetAccessListByTypeAsync(string accessType);

    Task<(ResponseStatus Status, AccessResponse? Response)> CreateAccessAsync(AccessRequestModel? accessRequest);

}

public sealed class AccessControllerService : IAccessControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IAccessRepository _repository;

    public AccessControllerService(
    ILogger<AccessControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IAccessRepository repository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
    }


    public async Task<(ResponseStatus Status, AccessResponse? Response)> GetAccessByNameAsync(string accessName)
    {
        Access? access;
       
        try
        {
            access =  await _repository.GetAccessByNameAsync(accessName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve access with code {accessName}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (access == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(access);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<AccessResponse>? Response)> GetAccessListByTypeAsync(string accessType)
    {
        List<Access>? accessList;
       
        try
        {
            accessList = await _repository.GetAccessListByTypeAsync(accessType);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve access list with type {accessType}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (accessList == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(accessList);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<AccessResponse>? Response)> GetAccessListAsync()
    {
        List<Access>? accessList;
       
        try
        {
            accessList = await _repository.GetAccessListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve access list");
            return (ResponseStatus.UnknownError, null);
        }

        if (accessList == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(accessList);

        return (ResponseStatus.Successful, response);
    }



    public async Task<(ResponseStatus Status, AccessResponse? Response)> CreateAccessAsync(AccessRequestModel? accessRequest)
    {
        if (accessRequest == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Access access = _requestMapper.Map(accessRequest);

        try
        {
            var found =  await _repository.GetAccessByNameAsync(access.AccessName);
            if (found == null)
            {
                access = await _repository.SaveAsync(access);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create access {access.AccessName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(access);

        return (ResponseStatus.Successful, response);
    }

}