using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenManagementSystem.Models
{
    public class TokenDetails
    {
        public int Id { get; set; }

        public Guid TokenNumber { get; set; }

        public int EstimatedWaitingTime { get; set; }

        public Guid Status { get; set; }

        public int Counter { get; set; }

        public string ServiceType { get; set; }
    }
}
