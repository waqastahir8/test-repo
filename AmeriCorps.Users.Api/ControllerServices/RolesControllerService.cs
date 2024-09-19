using AmeriCorps.Users.Data.Core;

namespace AmeriCorps.Users.Api;

public interface IRolesControllerService
{
    Task<(ResponseStatus Status, RoleResponse? Response)> GetAsync(int id);

    Task<(ResponseStatus Status, RoleResponse? Response)> CreateOrPatchAsync(RoleRequestModel? roleRequest);

    Task<(ResponseStatus Status, List<RoleResponse>? Response)> GetRoleListByTypeAsync(string roleType);
}

public sealed class RolesControllerService : IRolesControllerService
{
    private readonly ILogger<RolesControllerService> _logger;

    private readonly IRequestMapper _reqMapper;
    private readonly IResponseMapper _respMapper;
    private readonly IValidator _validator;
    private readonly IRoleRepository _repository;

    public RolesControllerService(
        ILogger<RolesControllerService> logger,
        IRequestMapper reqMapper,
        IResponseMapper respMapper,
        IValidator validator,
        IRoleRepository repository)
    {
        _logger = logger;
        _reqMapper = reqMapper;
        _respMapper = respMapper;
        _validator = validator;
        _repository = repository;
    }

    public async Task<(ResponseStatus Status, RoleResponse? Response)> GetAsync(int id)
    {
        Role? role;

        try
        {
            role = await _repository.GetAsync(id);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve role with id {Identifier}.", id.ToString().Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        if (role == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _respMapper.Map(role);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, List<RoleResponse>? Response)> GetRoleListByTypeAsync(string roleType)
    {
        List<Role>? roleList;

        try
        {
            roleList = await _repository.GetRoleListByTypeAsync(roleType);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not retrieve access list");
            return (ResponseStatus.UnknownError, null);
        }

        if (roleList == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _respMapper.Map(roleList);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, RoleResponse? Response)> CreateOrPatchAsync(RoleRequestModel? roleRequest)
    {
        if (roleRequest == null || !_validator.Validate(roleRequest))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Role? role = _reqMapper.Map(roleRequest);

        try
        {
            var existingRole = await _repository.GetAsync(role.Id);
            if (existingRole != null)
            {
                role = await _repository.UpdateRoleAsync(role);
            }
            else
            {
                role = await _repository.SaveAsync(role);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Unable to create or update role for {Identifier}.", roleRequest.RoleName.Replace(Environment.NewLine, ""));
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(role);

        return (ResponseStatus.Successful, response);
    }
}