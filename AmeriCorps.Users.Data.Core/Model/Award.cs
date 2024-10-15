using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(AwardCode))]
public sealed class Award : Entity
{

    [Column(TypeName = "varchar(16)")]
    public string AwardCode { get; set; } = string.Empty;
    [Column(TypeName = "varchar(64)")]
    public string AwardName { get; set; } = string.Empty;

    [Column(TypeName = "varchar(32)")]
    public string GspListingNumber { get; set; } = string.Empty;
    [Column(TypeName = "bigint")]
    public int Fain { get; set; }
    [Column(TypeName = "bigint")]
    public int Uei { get; set; }

    public DateOnly? PerformanceStartDt { get; set; }
    public DateOnly? PerformanceEndDt { get; set; }

}