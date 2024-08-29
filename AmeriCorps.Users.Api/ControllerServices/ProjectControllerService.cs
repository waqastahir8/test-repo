using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;


namespace AmeriCorps.Users.Api;

public interface IProjectControllerService
{

    Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCode(string projCode);

    Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProject(ProjectRequestModel? projRequest);

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


    public async Task<(ResponseStatus Status, ProjectResponse? Response)> GetProjectByCode(string projCode)
    {
        Project? project;
       
        try
        {
            project =  await _repository.GetProjectByCode(projCode);
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



    public async Task<(ResponseStatus Status, ProjectResponse? Response)> CreateProject(ProjectRequestModel? projRequest)
    {
        if (projRequest == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Project? project = _requestMapper.Map(projRequest);

        try
        {
            var foundProj = await _repository.GetProjectByCode(project.ProjectCode);
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