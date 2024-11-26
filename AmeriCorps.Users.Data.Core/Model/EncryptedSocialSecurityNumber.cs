namespace AmeriCorps.Users.Data.Core.Model;

public sealed class EncryptedSocialSecurityNumber : EntityWithUserId
{
    public string? SociaSecurityNumber { get; set; } = string.Empty;
}

