namespace AmeriCorps.Users.Models;

public sealed class UserSearchListResponseModel {
    public int UserId { get; set; }
    public List<SavedSearchResponseModel> Searches { get; set; } = [];
}