using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;


namespace AmeriCorps.Users.Api;

public interface IOrgControllerService
{

    Task<(ResponseStatus Status, OrganizationResponse? Response)> GetOrgByCodeAsync(string orgCode);

    Task<(ResponseStatus Status, List<OrganizationResponse>? Response)> GetOrgListAsync();

    Task<(ResponseStatus Status, OrganizationResponse? Response)> CreateOrgAsync(OrganizationRequestModel? orgRequest);

}

public sealed class OrgControllerService : IOrgControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IOrganizationRepository _repository;

    public OrgControllerService(
    ILogger<OrgControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IOrganizationRepository repository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
    }


    public async Task<(ResponseStatus Status, OrganizationResponse? Response)> GetOrgByCodeAsync(string orgCode)
    {
        Organization? organization;
       
        try
        {
            organization =  await _repository.GetOrgByCodeAsync(orgCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve org with code {orgCode}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (organization == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(organization);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<OrganizationResponse>? Response)> GetOrgListAsync()
    {
        List<Organization>? organization;
       
        try
        {
            organization =  await _repository.GetOrgListAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve org list.");
            return (ResponseStatus.UnknownError, null);
        }

        if (organization == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(organization);

        return (ResponseStatus.Successful, response);
    }




    public async Task<(ResponseStatus Status, OrganizationResponse? Response)> CreateOrgAsync(OrganizationRequestModel? orgRequest)
    {
        if (orgRequest == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Organization org = _requestMapper.Map(orgRequest);

        try
        {
            var foundOrg =  await _repository.GetOrgByCodeAsync(org.OrgCode);
            if (foundOrg == null)
            {
                org = await _repository.SaveAsync(org);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create org {orgRequest.OrgName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(org);

        return (ResponseStatus.Successful, response);
    }

}