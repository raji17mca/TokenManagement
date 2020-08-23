using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenManagementSystem.Models
{
    public class Customer
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public int AccountNumber { get; set; }

        public string SocialNumber { get; set; }

        public string CustomerType { get; set; }
    }
}
