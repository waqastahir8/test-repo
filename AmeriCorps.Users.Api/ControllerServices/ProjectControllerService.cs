using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api;

public interface IProjectControllerService
{
    Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCodeAsync(string projCode);

    Task<(ResponseStatus Status, List<ProjectResponse>? Response)> GetProjectListByOrgAsync(string orgCode);

    Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProjectAsync(ProjectRequestModel? projRequest);

    Task<(ResponseStatus Status, ProjectResponse? Response)> UpdateProjectAsync(ProjectRequestModel? projRequest);

    Task<(ResponseStatus Status, OperatingSiteResponse? Response)> UpdateOperatingSiteAsync(OperatingSiteRequestModel opSiteRequest);

    Task<(ResponseStatus Status, OperatingSiteResponse? Response)> InviteOperatingSiteAsync(OperatingSiteRequestModel toInvite);

    Task<(ResponseStatus Status, List<ProjectResponse>? Response)> SearchProjectsAsync(SearchFilters filters);

}

public sealed class ProjectControllerService : IProjectControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IProjectRepository _repository;
    private readonly IUserHelperService _userHelperService;

    public ProjectControllerService(
    ILogger<ProjectControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IProjectRepository repository,
    IUserHelperService userHelperService)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
        _userHelperService = userHelperService;
    }

    public async Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCodeAsync(string projCode)
    {
        Project? project;

        try
        {
            project = await _repository.GetProjectByCodeAsync(projCode);
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

        var response = _responseMapper.Map(project);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<ProjectResponse>? Response)> GetProjectListByOrgAsync(string orgCode)
    {
        List<Project>? projList;

        try
        {
            projList = await _repository.GetProjectListByOrgAsync(orgCode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve project list with orgCode {orgCode}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (projList == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(projList);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProjectAsync(ProjectRequestModel? projRequest)
    {
        if (projRequest == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Project? project = _requestMapper.Map(projRequest);

        try
        {
            var foundProj = await _repository.GetProjectByCodeAsync(project.ProjectCode);
            if (foundProj == null)
            {
                project = await _repository.SaveAsync(project);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to create project {projRequest.ProjectName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(project);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, ProjectResponse? Response)> UpdateProjectAsync(ProjectRequestModel? projRequest)
    {
        if (projRequest == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Project? updatedProject = _requestMapper.Map(projRequest);

        try
        {
            var foundProj = await _repository.GetProjectByCodeAsync(projRequest.ProjectCode);
            if (foundProj != null)
            {
                updatedProject.Id = foundProj.Id;

                if (foundProj.AuthorizedRep != null)
                {
                    updatedProject.AuthorizedRep = foundProj.AuthorizedRep;
                }

                if (foundProj.ProjectDirector != null)
                {
                    updatedProject.ProjectDirector = foundProj.ProjectDirector;
                }

                await _repository.UpdateProjectAsync(updatedProject);
            }
            else
            {
                _logger.LogInformation("No project named {Identifier} found to updated", projRequest.ProjectName.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.MissingInformation, null);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error to updating project {Identifier}.", projRequest.ProjectName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(updatedProject);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, OperatingSiteResponse? Response)> UpdateOperatingSiteAsync(OperatingSiteRequestModel opSiteRequest)
    {
        if (opSiteRequest == null || string.IsNullOrEmpty(opSiteRequest.Id.ToString()) || opSiteRequest.Id < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        OperatingSite updatedSite = _requestMapper.Map(opSiteRequest);

        try
        {
            var foundSite = await _repository.GetOperatingSiteByIdAsync(opSiteRequest.Id);
            if (foundSite != null)
            {
                updatedSite.Id = foundSite.Id;
                await _repository.SaveAsync(updatedSite);
            }
            else if (!string.IsNullOrEmpty(opSiteRequest.ProjectCode))
            {
                var foundProj = await _repository.GetProjectByCodeAsync(opSiteRequest.ProjectCode);

                if (foundProj != null)
                {
                    foundProj.OperatingSites.Add(updatedSite);
                    await _repository.UpdateProjectAsync(foundProj);
                }
            }
            else
            {
                _logger.LogInformation("No Operating Site or Project found to update operating sitenamed {Identifier}.", opSiteRequest.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.MissingInformation, null);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error to updating Operating Site {Identifier}.", opSiteRequest.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var response = _responseMapper.Map(updatedSite);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, OperatingSiteResponse? Response)> InviteOperatingSiteAsync(OperatingSiteRequestModel toInvite)
    {
        if (toInvite == null || string.IsNullOrEmpty(toInvite.Id.ToString()) || toInvite.Id < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        OperatingSite operatingSite = _requestMapper.Map(toInvite);

        if (operatingSite != null)
        {
            operatingSite.UpdatedDate = DateTime.UtcNow;

            operatingSite.InviteDate = DateTime.UtcNow;
        }
        else
        {
            return (ResponseStatus.UnknownError, null);
        }

        try
        {
            await _repository.SaveAsync(operatingSite);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving new Operating Site {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var success = false;
        try
        {
            success = await _userHelperService.SendOperatingSiteInviteAsync(operatingSite);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send invite for Operating Site {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (!success)
        {
            _logger.LogInformation("Invite email not sent for Operating Site {Identifier}.", toInvite.OperatingSiteName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(operatingSite);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<ProjectResponse>? Response)> SearchProjectsAsync(SearchFilters filters)
    {
        if (filters == null || string.IsNullOrEmpty(filters.Query) || string.IsNullOrEmpty(filters.OrgCode))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        List<Project>? projList;

        try
        {
            if(filters.Awarded){
                projList = await _repository.SearchAwardedProjectsAsync(filters.Query, filters.Active, filters.OrgCode);
            }
            else
            {
                projList = await _repository.SearchAllProjectsAsync(filters.Query, filters.Active, filters.OrgCode);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve project list with query {Identifier}.", filters.Query.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (projList == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(projList);

        return (ResponseStatus.Successful, response);
    }

}