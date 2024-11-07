using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Models;

public sealed class UserResponse : UserRequestModel
{
    public int Id { get; set; }
    public SocialSecurityVerificationResponse SocialSecurityVerification { get; set; }
}