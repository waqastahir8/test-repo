using System.ComponentModel.DataAnnotations;

namespace AmeriCorps.Users.Api.Models;

public sealed class EducationRequestModel
{
    public int Id { get; set; }
    public string Level { get; set; } = string.Empty;
    public string MajorAreaOfStudy { get; set; } = string.Empty;
    public string Institution { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public DateOnly DateAttendedFrom { get; set; }
    public DateOnly DateAttendedTo { get; set; }
    public string DegreeTypePursued { get; set; } = string.Empty;
    public bool DegreeCompleted { get; set; }
}
