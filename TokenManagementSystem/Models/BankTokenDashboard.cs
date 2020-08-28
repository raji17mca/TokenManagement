using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenManagementSystem.Models
{
    public class BankTokenDashboard
    {
        public string Id { get; set; }

        public int TokenNumber { get; set; }

        public string ServiceType { get; set; }

        public string Status { get; set; }
    }
}
