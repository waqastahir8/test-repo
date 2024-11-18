using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace AmeriCorps.Users.Data.Core;

[Index(nameof(GranteeCode))]
public sealed class SubGrantee : Entity
{
    [Column(TypeName = "varchar(32)")]
    public string GranteeCode { get; set; } = string.Empty;

    [Column(TypeName = "varchar(64)")]
    public string GranteeName { get; set; } = string.Empty;

    [Column(TypeName = "bigint")]
    public int Uei { get; set; }

    [Column(TypeName = "varchar(32)")]
    public string StreetAddress { get; set; } = string.Empty;

    [Column(TypeName = "varchar(32)")]
    public string City { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string State { get; set; } = string.Empty;

    [Column(TypeName = "varchar(8)")]
    public string ZipCode { get; set; } = string.Empty;
    
    public double AwardedMsys { get; set; }

    public double LivingAllowanceMsys { get; set; }
    public double NonLivingAllowanceMsys { get; set; }
}