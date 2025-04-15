using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace E_CommerceFIdentityScaff.Models
{
    //[PrimaryKey(nameof(ProductId), nameof(ApplicationUserId))]

    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ValidateNever] 
        public Product Product { get; set; }

        [Range(0, 1000,ErrorMessage ="Enter a value between 0 and 1000!")]
        public int count { get; set; }
        public string ApplicationUserId { get; set; }

        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
