namespace AmeriCorps.Users.Models;

public class EncryptedSocialSecurityNumberResponse : EncryptedSocialSecurityNumberRequestModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
}