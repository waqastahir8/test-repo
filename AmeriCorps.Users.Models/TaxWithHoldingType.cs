using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;
public enum TaxWithHoldingType
{
    SingleOrMarriedFilingSeparately = 0,
    MarriedOrFilingJointlyOrQualifySpouse = 1,
    HeadOfHousehold = 2,
}
