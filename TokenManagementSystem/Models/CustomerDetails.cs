using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TokenManagementSystem.Models
{
    public class CustomerDetails
    {
        [Key]
        [JsonProperty( PropertyName = "id")]
        public string Id { get; set; }
       
        [Required]
        public string Name { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public int AccountNumber { get; set; }

        [Required]
        public string SocialNumber { get; set; }

        [Required]
        public string CustomerType { get; set; }

        [Required]
        public string ServiceType { get; set; }

        public int TokenNumber { get; set; }

        public string Status { get; set; }

        public int? Counter { get; set; }
        
    }
}
