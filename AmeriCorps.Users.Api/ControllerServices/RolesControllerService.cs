using AmeriCorps.Users.Data.Core;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace AmeriCorps.Users.Api.ControllerServices
{
    public interface IRolesControllerService
    {
        Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id);

        Task<(ResponseStatus Status, UserResponse? Response)> CreateOrPatchAsync(UserRequestModel? userRequest);


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

        public async Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id)
        {
            Role? role;

            try
            {
                role = await _repository.GetAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Could not retrieve user with id {id}.");
                return (ResponseStatus.UnknownError, null);
            }

            if (role == null)
            {
                return (ResponseStatus.MissingInformation, null);
            }

            var response = _respMapper.Map(role);

            return (ResponseStatus.Successful, response);
        }


        public Task<(ResponseStatus Status, UserResponse? Response)> CreateOrPatchAsync(UserRequestModel? userRequest)
        {
            throw new NotImplementedException();
        }


    }

}
