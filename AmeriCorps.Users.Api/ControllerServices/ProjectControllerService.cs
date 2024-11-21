using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api;

public interface IProjectControllerService
{
    Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCodeAsync(string projCode);

    Task<(ResponseStatus Status, List<ProjectResponse>? Response)> GetProjectListByOrgAsync(string orgCode);

    Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProjectAsync(ProjectRequestModel? projRequest);

    Task<(ResponseStatus Status, ProjectResponse? Response)> UpdateProjectAsync(ProjectRequestModel? projRequest);

    Task<(ResponseStatus Status, OperatingSiteResponse? Response)> UpdateOperatingSiteAsync(OperatingSiteRequestModel opSiteRequest);

    Task<(ResponseStatus Status, ProjectResponse? Response)> InviteOperatingSiteAsync(ProjectRequestModel toInvite);

    Task<(ResponseStatus Status, List<ProjectResponse>? Response)> SearchProjectsAsync(SearchFiltersRequestModel filters);

    Task<(ResponseStatus Status, List<OperatingSiteResponse>? Response)> SearchOperatingSitesAsync(SearchFiltersRequestModel filters);
}

public sealed class ProjectControllerService : IProjectControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IProjectRepository _repository;
    private readonly IUserHelperService _userHelperService;
    private readonly IUserRepository _userRepository;

    public ProjectControllerService(
    ILogger<ProjectControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IProjectRepository repository,
    IUserHelperService userHelperService,
    IUserRepository userRepository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
        _userHelperService = userHelperService;
        _userRepository = userRepository;
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

        double proposedMsyUsage = project.OperatingSites.Sum(x => x.AwardedMsys);

        if (proposedMsyUsage > project.TotalAwardedMsys)
        {
            _logger.LogWarning($"The specified operating site MSYs results in MSY higher than awarded MSYs to project.");
            return (ResponseStatus.MissingInformation, null);
        }

        try
        {
            var foundProj = await _repository.GetProjectByCodeAsync(project.ProjectCode);
            if (foundProj == null)
            {
                project = await _repository.SaveAsync(project);
            }
            else
            {
                project = foundProj;
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

        double proposedMsyUsage = projRequest.OperatingSites.Sum(x => x.AwardedMsys);

        if (proposedMsyUsage > projRequest.TotalAwardedMsys)
        {
            _logger.LogWarning($"The specified operating site MSYs results in MSY higher than awarded MSYs to project.");
            return (ResponseStatus.MissingInformation, null);
        }

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

                if (foundProj.Award != null)
                {
                    updatedProject.Award = foundProj.Award;
                }

                if (foundProj.SubGrantees != null)
                {
                    updatedProject.SubGrantees = foundProj.SubGrantees;
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

        var project = await _repository.GetProjectByCodeAsync(opSiteRequest.ProjectCode);

        if (project == null)
        {
            _logger.LogWarning($"Operating site not created. Project with id {opSiteRequest.ProjectCode} was not found.");
            return (ResponseStatus.MissingInformation, null);
        }

        double proposedMsyUsage = project.OperatingSites.Sum(x => x.AwardedMsys) + opSiteRequest.AwardedMsys;

        if (proposedMsyUsage > project?.TotalAwardedMsys)
        {
            _logger.LogWarning($"The specified operating site MSYs results in MSY higher than awarded MSYs to project.");
            return (ResponseStatus.MissingInformation, null);
        }

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

    public async Task<(ResponseStatus Status, ProjectResponse? Response)> InviteOperatingSiteAsync(ProjectRequestModel toInvite)
    {
        if (toInvite == null || string.IsNullOrEmpty(toInvite.Id.ToString()) || toInvite.OperatingSites == null || toInvite.OperatingSites.Count < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var foundProject = await _repository.GetProjectByCodeAsync(toInvite.ProjectCode);
        OperatingSite inviteSite = _requestMapper.Map(toInvite.OperatingSites[0]);

        if (foundProject != null && inviteSite != null)
        {
            inviteSite.UpdatedDate = DateTime.UtcNow;
            inviteSite.InviteDate = DateTime.UtcNow;

            inviteSite = await CreateOperatingSiteContactAsync(inviteSite, foundProject);

            if (string.IsNullOrEmpty(inviteSite.Id.ToString()) || inviteSite.Id < 1)
            {
                foundProject.OperatingSites.Add(inviteSite);
            }
            else
            {
                foundProject.OperatingSites = foundProject.OperatingSites.Where(x => x.Id != inviteSite.Id).ToList();
                foundProject.OperatingSites.Add(inviteSite);
            }
        }
        else
        {
            return (ResponseStatus.UnknownError, null);
        }

        try
        {
            foundProject = await _repository.UpdateProjectAsync(foundProject);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving new Operating Site for {Identifier}.", toInvite.ProjectName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var success = false;
        try
        {
            if (foundProject != null && foundProject.OperatingSites != null && foundProject.OperatingSites.Count > 0)
            {
                foundProject.OperatingSites = foundProject.OperatingSites.OrderBy(s => s.UpdatedDate).ToList();
                success = await _userHelperService.SendOperatingSiteInviteAsync(foundProject.OperatingSites[foundProject.OperatingSites.Count - 1]);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to send invite for Operating Site for {Identifier}.", toInvite.ProjectName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (!success)
        {
            _logger.LogInformation("Invite email not sent for Operating Site for {Identifier}.", toInvite.ProjectName.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _responseMapper.Map(foundProject);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<ProjectResponse>? Response)> SearchProjectsAsync(SearchFiltersRequestModel filters)
    {
        List<Project> projectList = new List<Project>();

        if (filters == null || string.IsNullOrEmpty(filters.Query) || string.IsNullOrEmpty(filters.OrgCode))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        List<Project>? foundList;
        try
        {
            if (filters.Awarded && filters.Active)
            {
                foundList = await _repository.SearchActiveAwardedProjectsAsync(filters.Query.Trim() + ":*", filters.Active, filters.OrgCode);
            }
            else if (filters.Awarded && !filters.Active)
            {
                foundList = await _repository.SearchAwardedProjectsAsync(filters.Query.Trim() + ":*", filters.OrgCode);
            }
            else
            {
                foundList = await _repository.SearchAllProjectsAsync(filters.Query.Trim() + ":*", filters.OrgCode);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve project list with query {Identifier}.", filters.Query.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (foundList != null)
        {
            foundList.ForEach(proj =>
            {
                projectList.Add(proj);
            });
        }

        var response = _responseMapper.Map(projectList);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<OperatingSiteResponse>? Response)> SearchOperatingSitesAsync(SearchFiltersRequestModel filters)
    {
        List<OperatingSite> opSiteList = new List<OperatingSite>();
        if (filters == null || string.IsNullOrEmpty(filters.Query) || filters.ProjectId < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        List<OperatingSite>? foundList;

        try
        {
            foundList = await _repository.SearchOperatingSitesAsync(filters.ProjectId, filters.Active, filters.Query.Trim() + ":*");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve project list with query {Identifier}.", filters.Query.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (foundList != null)
        {
            foundList.ForEach(site =>
            {
                opSiteList.Add(site);
            });
        }

        var response = _responseMapper.Map(opSiteList);

        return (ResponseStatus.Successful, response);
    }


    private async Task<OperatingSite> CreateOperatingSiteContactAsync(OperatingSite inviteSite, Project project)
    {
        User toInvite = new User();

        if(!string.IsNullOrEmpty(inviteSite.ContactName))
        {
            var contactName = inviteSite.ContactName.Split(' ', 2);

            toInvite.FirstName = contactName[0];
            toInvite.LastName = contactName[1];
        }

        toInvite.OrgCode = project.ProjectOrgCode;
        toInvite.InviteUserId = inviteSite.InviteUserId;
        toInvite.InviteDate = DateTime.UtcNow;

        CommunicationMethod email = new CommunicationMethod()
        {
            Type = "email",
            Value = inviteSite.EmailAddress,
            IsPreferred = true
        };

        List<CommunicationMethod> commList = new List<CommunicationMethod>();
        commList.Add(email);
        toInvite.CommunicationMethods = commList;
        toInvite.UserName = inviteSite.EmailAddress;


        UserProject userProject = new UserProject()
        {
            ProjectName = project.ProjectName,
            ProjectCode = project.ProjectCode,
            ProjectType = project.ProjectType,
            ProjectOrg = project.ProjectOrgCode,
            Active = true
        };

        List<UserProject> projects = new List<UserProject>();
        projects.Add(userProject);

        toInvite.UserProjects = projects;

        try
        {
            toInvite = await _userRepository.SaveAsync(toInvite);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to save contact for new operating site {inviteSite}.");
            return (inviteSite);
        }

        if(toInvite != null && toInvite.Id > 1)
        {
            inviteSite.Contact = toInvite;
        }

        return inviteSite;
    }
}