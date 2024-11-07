using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;

public class SocialSecurityVerificationRequestModel
{
    public int UserId { get; set; }
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

