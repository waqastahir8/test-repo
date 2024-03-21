using FluentValidation;
using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;


namespace AmeriCorps.Users.Api;

public interface IUsersControllerService
{
    Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id);

    Task<(ResponseStatus Status, UserResponse? Response)> CreateAsync(UserRequestModel userRequest);
}
public sealed class UsersControllerService(
    ILogger<UsersControllerService> logger,
    IRequestMapper reqMapper,
    IResponseMapper respMapper,
    IValidator<UserRequestModel> validator,
    IUserRepository repository)
    : IUsersControllerService
{
    private readonly ILogger<UsersControllerService> _logger = logger;

    private readonly IRequestMapper _reqMapper = reqMapper;
    private readonly IResponseMapper _respMapper = respMapper;
    private readonly IValidator<UserRequestModel> _validator = validator;
    private readonly IUserRepository _repository = repository;
    public async Task<(ResponseStatus Status, UserResponse? Response)> GetAsync(int id) {
       
        User? user;
        
        try {
            user = await _repository.GetAsync(id);
        } catch(Exception e) {
            _logger.LogError(e, $"Could not retrieve user with id {id}");
            user = null;
        }

        if (user == null) {
            return (ResponseStatus.UnknownError,null);
        }

        var response = _respMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> 
                                                    CreateAsync(UserRequestModel userRequest){

        var validationResult = await _validator.ValidateAsync(userRequest);

        if (userRequest == null || !validationResult.IsValid) {
            return (ResponseStatus.MissingInformation, null);
        }

        User? user = _reqMapper.Map(userRequest);

        try {
            user = await _repository.CreateAsync(user);
        } catch(Exception e) {
            _logger.LogError(e, $"Unable to create user for {userRequest.LastName}, {userRequest.FirstName}.");
            user = null;
        }

        if (user == null) {
            return (ResponseStatus.UnknownError,null);
        }

        var response = _respMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }
}