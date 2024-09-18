using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;


namespace AmeriCorps.Users.Api;

public interface IProjectControllerService
{

    Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCodeAsync(string projCode);

    Task<(ResponseStatus Status, List<ProjectResponse>? Response)> GetProjectListByOrgAsync(string orgCode);

    Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProjectAsync(ProjectRequestModel? projRequest);

}

public sealed class ProjectControllerService : IProjectControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IProjectRepository _repository;

    public ProjectControllerService(
    ILogger<ProjectControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IProjectRepository repository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _repository = repository;
    }


    public async Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCodeAsync(string projCode)
    {
        Project? project;
       
        try
        {
            project =  await _repository.GetProjectByCodeAsync(projCode);
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

    public async  Task<(ResponseStatus Status, List<ProjectResponse>? Response)> GetProjectListByOrgAsync(string orgCode)
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
            }else
            {

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

}