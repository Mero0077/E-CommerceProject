using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceFIdentityScaff.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        public string Author {  get; set; }

        [ValidateNever]
        public string? ImagePath { get; set; }

        [Required]
        [Range(1,1000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 1000)]
        public double Price { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name="Price for +50")]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 1000)]
        [Display(Name = "Price for +100")]
        public double Price100 { get; set; }

        [ValidateNever]
        public Category Category { get; set; }
        public int CategoryId { get; set; }


    }
}
