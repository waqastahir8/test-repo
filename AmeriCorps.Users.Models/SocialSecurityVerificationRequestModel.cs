using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;

public class SocialSecurityVerificationRequestModel
{
    public int UserId { get; set; }
    public string SocialSecurity { get; set; }
    public VerificationStatusResponse CitizenshipStatus { get; set; } = new VerificationStatusResponse();
    public VerificationStatusResponse SocialSecurityStatus { get; set; } = new VerificationStatusResponse();
    public string VerificationCode { get; set; } = string.Empty;
    public string CitizenshipCode { get; set; } = string.Empty;
    public DateTime? ProcessStartDate { get; set; }
    public DateTime? CitizenshipUpdatedDate { get; set; }
    public DateTime? SocialSecurityUpdatedDate { get; set; }
    public int SubmitCount { get; set; } = 0;
    public int LastSubmitUser { get; set; }
    public SSAFileStatusRequestModel FileStatus { get; set; } = new SSAFileStatusRequestModel();
    public string? SSAVerificationTaskId { get; set; }
}

