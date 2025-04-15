﻿using System.ComponentModel.DataAnnotations;

namespace E_CommerceFIdentityScaff.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required] [MaxLength(30)]
        public string Name { get; set; }
        [Range(1,100)]
        public int DisplayOrder { get; set; }

        public List<Product> Products { get; set; }
    }
}
