using System.ComponentModel.DataAnnotations;

namespace E_CommerceFIdentityScaff.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public string? Number { get; set; }

        public ICollection<ApplicationUser> Users { get; set; }
    }
}
