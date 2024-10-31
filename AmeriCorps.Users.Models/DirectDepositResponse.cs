using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;
public sealed class DirectDepositResponse : DirectDepositRequestModel
{
    public int Id { get; set; }
    public int UserId { get; set; }

}
