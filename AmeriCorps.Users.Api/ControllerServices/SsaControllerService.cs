using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;

namespace AmeriCorps.Users.Api;

public interface ISsaControllerService
{
    Task<(ResponseStatus Status, bool? Response)> BulkUpdateVerificationDataAsync(List<SocialSecurityVerificationRequestModel> updateList);

    Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfo(int userId, SocialSecurityVerificationRequestModel verificationUpdate);

    Task<(ResponseStatus Status, UserResponse? Response)> SubmitInfoForVerificationAsync(int userId);

    Task<(ResponseStatus Status, List<UserResponse>? Response)> FetchPendingUsersForSSAVerificationAsync();
}

public sealed class SsaControllerService : ISsaControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IUserRepository _userRepository;
    private readonly ISocialSecurityVerificationRepository _ssvRepository;
    private readonly IEncryptionService _encryptionService;

    public SsaControllerService(
    ILogger<SsaControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IUserRepository userRepository,
    IEncryptionService encryptionService,
    ISocialSecurityVerificationRepository ssvRepository)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _userRepository = userRepository;
        _ssvRepository = ssvRepository;
        _encryptionService = encryptionService;
    }


    public async Task<(ResponseStatus Status, bool? Response)> BulkUpdateVerificationDataAsync(List<SocialSecurityVerificationRequestModel> updateList)
    {
        if (updateList == null || updateList.Count < 1)
        {
            return (ResponseStatus.MissingInformation, false);

        }

        for (int i = 0; i < updateList.Count; i++)
        {
            var encryptedId = _encryptionService.Encrypt(updateList[i].SocialSecurity);

            if (!string.IsNullOrWhiteSpace(encryptedId))
            {
                User? foundUser = new User();
                try
                {
                    foundUser = await _userRepository.FetchUserByEncryptedSSNAsync(encryptedId);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Error searching for user with encrypted ID for SSA update {Identifier}.", encryptedId.ToString().Replace(Environment.NewLine, ""));
                }

                if (foundUser != null && foundUser.SocialSecurityVerification != null)
                {

                    if (foundUser.SocialSecurityVerification.SocialSecurityStatus != VerificationStatus.Verified)
                    {
                        foundUser.SocialSecurityVerification.SocialSecurityStatus = (VerificationStatus)updateList[i].SocialSecurityStatus;
                        foundUser.SocialSecurityVerification.VerificationCode = updateList[i].VerificationCode;
                        foundUser.SocialSecurityVerification.SocialSecurityUpdatedDate = DateTime.UtcNow;
                    }

                    if (foundUser.SocialSecurityVerification.CitizenshipStatus != VerificationStatus.Verified)
                    {
                        foundUser.SocialSecurityVerification.CitizenshipStatus = (VerificationStatus)updateList[i].CitizenshipStatus;
                        foundUser.SocialSecurityVerification.CitizenshipCode = updateList[i].CitizenshipCode;
                        foundUser.SocialSecurityVerification.CitizenshipUpdatedDate = DateTime.UtcNow;
                    }

                    foundUser.SocialSecurityVerification.FileStatus = SSAFileStatus.OnFile;

                    try
                    {
                        await _ssvRepository.SaveAsync(foundUser.SocialSecurityVerification);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e, "Error updates social security verification for user {Identifier}.", foundUser.UserName.ToString().Replace(Environment.NewLine, ""));
                    }
                }
            }
        }
        return (ResponseStatus.Successful, true);
    }


    public async Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfo(int userId, SocialSecurityVerificationRequestModel verificationUpdate)
    {
        if (verificationUpdate == null || userId < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SocialSecurityVerification? userStatus;

        try
        {
            userStatus = await _userRepository.FindSocialSecurityVerificationByUserId(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if verification for {userId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if(userStatus != null){
            userStatus.LastSubmitUser = verificationUpdate.LastSubmitUser;
            if(userStatus.CitizenshipStatus != (VerificationStatus)verificationUpdate.CitizenshipStatus)
            {
                userStatus.CitizenshipStatus = (VerificationStatus)verificationUpdate.CitizenshipStatus;
                userStatus.CitizenshipUpdatedDate = DateTime.UtcNow;
            }
            if(userStatus.SocialSecurityStatus != (VerificationStatus)verificationUpdate.SocialSecurityStatus)
            {
                userStatus.SocialSecurityStatus = (VerificationStatus)verificationUpdate.SocialSecurityStatus;
                userStatus.SocialSecurityUpdatedDate = DateTime.UtcNow;
            }

            if((userStatus.SocialSecurityStatus == VerificationStatus.Resubmit || userStatus.CitizenshipStatus == VerificationStatus.Resubmit)&& userStatus.SubmitCount < 5){
                userStatus.SubmitCount++;

                userStatus.FileStatus = SSAFileStatus.PendingToSend;
            }

            try
            {
                userStatus = await _ssvRepository.SaveAsync(userStatus);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save update for user {userId} exists.");
                return (ResponseStatus.UnknownError, null);
            }
        }

        var response = _responseMapper.Map(userStatus);

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, UserResponse? Response)> SubmitInfoForVerificationAsync(int userId)
    {
        if (userId < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        User? user;
        try
        {
            user = await _userRepository.GetAsync(userId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Could not retrieve user with id {userId}.");
            return (ResponseStatus.UnknownError, null);
        }

        if (user == null || string.IsNullOrEmpty(user.EncryptedSocialSecurityNumber))
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SocialSecurityVerification userStatus =  new SocialSecurityVerification()
        {
            UserId = user.Id,
            CitizenshipStatus = VerificationStatus.Pending,
            SocialSecurityStatus = VerificationStatus.Pending,

            ProcessStartDate = DateTime.UtcNow,
            SubmitCount = 1,
            LastSubmitUser = 0,
            FileStatus = SSAFileStatus.PendingToSend
        };

        user.SocialSecurityVerification = userStatus;

        try
        {
            await _ssvRepository.SaveAsync(userStatus);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error saving new social security verification for user {Identifier}.", user.UserName.ToString().Replace(Environment.NewLine, ""));
        }

        var response = _responseMapper.Map(user);

        return (ResponseStatus.Successful, response);
    }


    public async Task<(ResponseStatus Status, List<UserResponse>? Response)> FetchPendingUsersForSSAVerificationAsync()
    {
        List<User>? userList;

        try
        {
            userList = await _userRepository.FetchPendingUsersForSSAVerificationAsync();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error fetching pending users for ssa verification.");
            return (ResponseStatus.UnknownError, null);
        }

        List<UserResponse> response = new List<UserResponse>();

        if(userList != null && userList.Count > 0)
        {
            for(int i = 0; i < userList.Count; i++)
            {
                var mapped = _responseMapper.Map(userList[i]);
                if(mapped != null)
                {
                    mapped.EncryptedSocialSecurityNumber = _encryptionService.Decrypt(mapped.EncryptedSocialSecurityNumber);
                    response.Add(mapped);
                }
            }
        }

        return (ResponseStatus.Successful, response);
    }

}