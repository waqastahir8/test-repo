using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;

public class TaxWithHoldingRequestModel
{
    public TaxWithHoldingType TaxWithHoldingType { get; set; }
    public string? Step2Box1 { get; set; } = string.Empty;
    public string? Step2Box2 { get; set; } = string.Empty;
    public string? Step3Box1 { get; set; } = string.Empty;
    public string? Step3Box2 { get; set; } = string.Empty;
    public string? Step4Box1 { get; set; } = string.Empty;
    public string? Step4Box2 { get; set; } = string.Empty;
    public string? Step4Box3 { get; set; } = string.Empty;
}