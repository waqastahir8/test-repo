using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmeriCorps.Users.Models;
public class AssignTemplateRequestModel
{
    //public long TemplateId { get; set; }
    //public List<int> AwardIds { get; set; } = new List<int>();

    public long TemplateId { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<int> AwardIds { get; set; } = new List<int>();
}
