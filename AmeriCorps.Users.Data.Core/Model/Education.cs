namespace AmeriCorps.Users.Data.Core;

public sealed class Education
{
    public int Id { get; set; }
    public int Level { get; set; }
    public int MajorAreaOfStudy { get; set; }
    public int Institution { get; set; }
    public int City { get; set; }
    public int State { get; set; }
    public DateTime DateAttendedFrom { get; set; }
    public DateTime DateAttendedTo { get; set; }
    public int DegreeTypePursued { get; set; }
    public bool DegreeCompleted { get; set; }
}
