
namespace AmeriCorps.Users.Models;

public sealed class SearchFiltersRequestModel
{
    public string OrgCode { get; set; } = string.Empty;
    public string Query { get; set; } = string.Empty;
    public bool Awarded { get; set; } = true;
    public bool Active { get; set; } = true;
    public int ProjectId { get; set; }
}