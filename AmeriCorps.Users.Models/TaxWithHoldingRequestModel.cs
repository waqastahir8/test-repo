using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;

public class TaxWithHoldingRequestModel
{
    public TaxWithHoldingType TaxWithHoldingType { get; set; }
    public string? AdditionalWithHoldings { get; set; } = string.Empty;
    public string? AdditionalWithHoldings2 { get; set; } = string.Empty;
    public string? DependentsUnder17 { get; set; } = string.Empty;
    public string? DependentsOver17 { get; set; } = string.Empty;
    public string? OtherIncome { get; set; } = string.Empty;
    public string? Deductions { get; set; } = string.Empty;
    public string? ExtraWithHoldingAmount { get; set; } = string.Empty;
    public DateTime? ModifiedDate { get; set; }
}