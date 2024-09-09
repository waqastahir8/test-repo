namespace AmeriCorps.Users.Models;

public class CollectionResponseModel
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    
    public int ListingId { get; set; }

    public string Type { get; set; } = string.Empty;
}