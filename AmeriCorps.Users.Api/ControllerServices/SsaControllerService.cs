using AmeriCorps.Users.Data.Core;
using AmeriCorps.Users.Data;
using AmeriCorps.Users.Models;
using AmeriCorps.Users.Api.Services;
using System.Data;

namespace AmeriCorps.Users.Api;

public interface ISsaControllerService
{
    Task<(ResponseStatus Status, bool? Response)> BulkUpdateVerificationDataAsync(List<SocialSecurityVerificationRequestModel> updateList);

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

}