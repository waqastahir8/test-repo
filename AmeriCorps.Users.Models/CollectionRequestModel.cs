namespace AmeriCorps.Users.Models;

public class CollectionRequestModel
{
  
    public int UserId { get; set; }
    
    public int ListingId { get; set; }

    public string Type { get; set; } = string.Empty;
    
    
}