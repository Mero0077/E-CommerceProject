﻿using Microsoft.AspNetCore.Identity;

namespace E_CommerceFIdentityScaff.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Address { get; set; }
    }
}
