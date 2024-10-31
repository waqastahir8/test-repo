using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;
public class DirectDepositRequestModel
{
    public AccountType AccountType { get; set; }
    public string InstitutionName { get; set; } = string.Empty;
    public string AchRoutingNumber { get; set; } = string.Empty;
    public string ReEnterAchRoutingNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string ReEnterAccountNumber { get; set; } = string.Empty;
    public bool MailByPaycheck { get; set; }
}
