using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;

namespace AmeriCorps.Users.Api;

public interface ISsaControllerService
{
    Task<(ResponseStatus Status, bool? Response)> BulkUpdateVerificationDataAsync(List<SocialSecurityVerificationRequestModel> updateList);

    Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfoAsync(SocialSecurityVerificationRequestModel verificationUpdate);

    Task<(ResponseStatus Status, UserResponse? Response)> SubmitInfoForVerificationAsync(int userId);

    Task<(ResponseStatus Status, List<UserResponse>? Response)> FetchPendingUsersForSSAVerificationAsync();

    Task<(ResponseStatus Status, bool? Response)> NotifyFailedUserVerificationsAsync(int userId);
}

public sealed class SsaControllerService : ISsaControllerService
{
    private readonly ILogger _logger;
    private readonly IResponseMapper _responseMapper;
    private readonly IRequestMapper _requestMapper;
    private readonly IUserRepository _userRepository;
    private readonly ISocialSecurityVerificationRepository _ssvRepository;
    private readonly IEncryptionService _encryptionService;
    private readonly IUserHelperService _userHelperService;

    public SsaControllerService(
    ILogger<SsaControllerService> logger,
    IRequestMapper requestMapper,
    IResponseMapper responseMapper,
    IUserRepository userRepository,
    IEncryptionService encryptionService,
    ISocialSecurityVerificationRepository ssvRepository,
    IUserHelperService userHelperService)
    {
        _logger = logger;
        _requestMapper = requestMapper;
        _responseMapper = responseMapper;
        _userRepository = userRepository;
        _ssvRepository = ssvRepository;
        _encryptionService = encryptionService;
        _userHelperService = userHelperService;
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


    public async Task<(ResponseStatus Status, SocialSecurityVerificationResponse? Response)> UpdateUserSSAInfoAsync(SocialSecurityVerificationRequestModel verificationUpdate)
    {
        if (verificationUpdate == null || verificationUpdate.UserId < 1)
        {
            return (ResponseStatus.MissingInformation, null);
        }

        SocialSecurityVerification? userStatus;

        try
        {
            userStatus = await _userRepository.FindSocialSecurityVerificationByUserId(verificationUpdate.UserId);
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Unable to check if verification for {verificationUpdate.UserId} exists.");
            return (ResponseStatus.UnknownError, null);
        }

        if (userStatus != null)
        {
            userStatus.LastSubmitUser = verificationUpdate.LastSubmitUser;
            if (userStatus.CitizenshipStatus != (VerificationStatus)verificationUpdate.CitizenshipStatus)
            {
                userStatus.CitizenshipStatus = (VerificationStatus)verificationUpdate.CitizenshipStatus;
                userStatus.CitizenshipUpdatedDate = DateTime.UtcNow;
            }
            if (userStatus.SocialSecurityStatus != (VerificationStatus)verificationUpdate.SocialSecurityStatus)
            {
                userStatus.SocialSecurityStatus = (VerificationStatus)verificationUpdate.SocialSecurityStatus;
                userStatus.SocialSecurityUpdatedDate = DateTime.UtcNow;
            }

            if ((userStatus.SocialSecurityStatus == VerificationStatus.Resubmit || userStatus.CitizenshipStatus == VerificationStatus.Resubmit) && userStatus.SubmitCount < 5)
            {
                userStatus.SubmitCount++;

                userStatus.FileStatus = SSAFileStatus.PendingToSend;
            }

            try
            {
                userStatus = await _ssvRepository.SaveAsync(userStatus);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Unable to save update for user {verificationUpdate.UserId} exists.");
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

        if (user.SocialSecurityVerification == null)
        {
            SocialSecurityVerification userStatus = new SocialSecurityVerification()
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
        }
        else
        {
            user.SocialSecurityVerification.CitizenshipStatus = VerificationStatus.Pending;
            user.SocialSecurityVerification.SocialSecurityStatus = VerificationStatus.Pending;
            user.SocialSecurityVerification.ProcessStartDate = DateTime.UtcNow;
            user.SocialSecurityVerification.FileStatus = SSAFileStatus.PendingToSend;
        }


        try
        {
            await _ssvRepository.SaveAsync(user.SocialSecurityVerification);
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

        if (userList != null)
        {
            for (int i = 0; i < userList.Count; i++)
            {
                var mapped = _responseMapper.Map(userList[i]);
                if (mapped != null)
                {
                    mapped.EncryptedSocialSecurityNumber = _encryptionService.Decrypt(mapped.EncryptedSocialSecurityNumber);
                    response.Add(mapped);

                    await AddUserToFileAsync(userList[i]);
                }
            }
        }

        return (ResponseStatus.Successful, response);
    }

    public async Task<(ResponseStatus Status, bool? Response)> NotifyFailedUserVerificationsAsync(int userId)
    {
        List<User>? userList = new List<User>();

        if(userId > 0)
        {
            try
            {
                User? foundUser = await _userRepository.GetAsync(userId);
                if(foundUser != null)
                {
                    userList.Add(foundUser);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching user {Identifier} for ssa verification.", userId.ToString().Replace(Environment.NewLine, ""));
                return (ResponseStatus.UnknownError, null);
            }
        }
        else
        {
            try
            {
                userList = await _userRepository.FetchFailedSSAChecksAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error fetching user list for failed ssa verification.");
                return (ResponseStatus.UnknownError, null);
            }
        }

        if (userList == null)
        {
            return (ResponseStatus.MissingInformation, false);
        }

        var success = await _userHelperService.SendSSAFailureEmailAsync(userList);

        return (ResponseStatus.Successful, success);
    }

    private async Task<bool> AddUserToFileAsync(User user)
    {
        if (user != null && user.SocialSecurityVerification != null)
        {
            user.SocialSecurityVerification.FileStatus = SSAFileStatus.OnFile;


            try
            {
                await _ssvRepository.SaveAsync(user.SocialSecurityVerification);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving adding user to file for verification for user {Identifier}.", user.UserName.ToString().Replace(Environment.NewLine, ""));
                return false;
            }

            return true;
        }
        return false;
    }
}