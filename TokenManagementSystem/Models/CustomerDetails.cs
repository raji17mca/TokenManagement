using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using TokenManagementSystem.Filter;

namespace TokenManagementSystem.Models
{
    public class CustomerDetails
    {
        [Key]
        [JsonProperty( PropertyName = "id")]
        [SwaggerExclude]
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
        [RegularExpression(Constants.ServiceType.Service + "|" + Constants.ServiceType.BankTransaction, ErrorMessage = " Value must be Bank Transaction or Service")]
        public string ServiceType { get; set; }

        [SwaggerExclude]
        public int TokenNumber { get; set; }

        [SwaggerExclude]
        public string Status { get; set; }

        [SwaggerExclude]
        public int? Counter { get; set; }
        
    }
}
