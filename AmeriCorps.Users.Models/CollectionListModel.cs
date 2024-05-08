namespace AmeriCorps.Users.Models;

public abstract class CollectionListModel
{
    
    public int UserId { get;set;}

    public string Type {get;set;} = string.Empty;
    public List<int> Listings {get;set;} =new();
}