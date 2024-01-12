using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBSConnect.Business.Models
{
    public class CreateResetHistoryRequest
    {
        public int UserId { get; set; }
        public string SemesterName { get; set; }
    }
}

