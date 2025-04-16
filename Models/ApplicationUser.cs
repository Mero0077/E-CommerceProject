using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_CommerceFIdentityScaff.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
        public string? PhoneNumber {  get; set; }
        public string? City {  get; set; }
        public string? PostalCode { get; set; }
        public string? State { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company Company { get; set; }
    }
}
