using System;
using Microsoft.EntityFrameworkCore;


namespace AmeriCorps.Users.Data.Core;

public sealed class SocialSecurityVerification : EntityWithUserId
{
    public VerificationStatus CitizenshipStatus { get; set; } = new VerificationStatus();
    public VerificationStatus SocialSecurityStatus { get; set; } = new VerificationStatus();
    public string VerificationCode { get; set; } = string.Empty;
    public string CitizenshipCode { get; set; } = string.Empty;
    public DateTime? ProcessStartDate { get; set; }
    public DateTime? CitizenshipUpdatedDate { get; set; }
    public DateTime? SocialSecurityUpdatedDate { get; set; }
    public int SubmitCount { get; set; } = 0;
    public int LastSubmitUser { get; set; }
}