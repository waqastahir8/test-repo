namespace AmeriCorps.Users.Models;

public sealed class UserSearchesResponseModel {
    public int UserId { get; set; }
    public IEnumerable<SavedSearchResponseModel> Searches { get; set; } = [];
    
}