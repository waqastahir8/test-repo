using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;

public enum VerificationStatusResponse
{
    Pending = 0,
    Verified = 1,
    Returned = 2,
    Resubmit = 3,
    ManuallyVerified = 4,
    ManuallyReturned = 5,
    CannotBeManuallyVerified = 6,
    PendingManualVerification = 7,
    PreviouslyServed = 8,
    Error = 100,
}