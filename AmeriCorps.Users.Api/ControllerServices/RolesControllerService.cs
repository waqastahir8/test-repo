using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;


namespace AmeriCorps.Users.Api;

public interface IRolesControllerService
{
    Task<(ResponseStatus Status, RoleResponse? Response)> GetAsync(int id);

    Task<(ResponseStatus Status, RoleResponse? Response)> CreateOrPatchAsync(RoleRequestModel? roleRequest);
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
            _logger.LogError(e, $"Could not retrieve role with id {id}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (role == null)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        var response = _respMapper.Map(role);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, RoleResponse? Response)> CreateOrPatchAsync(RoleRequestModel? roleRequest)
    {
        if (roleRequest == null || !_validator.Validate(roleRequest))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        Role role = _reqMapper.Map(roleRequest);

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
            _logger.LogError(e, $"Unable to create or update role for {roleRequest.RoleName}.");
            return (ResponseStatus.UnknownError, null);
        }

        var response = _respMapper.Map(role);

        return (ResponseStatus.Successful, response);
    }

}
