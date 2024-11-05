namespace AmeriCorps.Users.Models;

public sealed class UserReferencesResponseModel
{
    public int UserId { get; set; }
    public IEnumerable<ReferenceResponseModel> References { get; set; } = [];
}