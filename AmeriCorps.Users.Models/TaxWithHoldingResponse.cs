using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;
public class TaxWithHoldingResponse : TaxWithHoldingRequestModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
}
