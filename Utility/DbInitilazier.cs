using E_CommerceFIdentityScaff.Data;
using E_CommerceFIdentityScaff.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace E_CommerceFIdentityScaff.Utility
{
    public class DbInitilazier : IDbInitilazier
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public DbInitilazier(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task Initialize()
        {


            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Company"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            var defaultAdminEmail = "omartalaatsaad2017@gmail.com";
            var adminUser = await _userManager.FindByEmailAsync(defaultAdminEmail);
            if (adminUser == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = defaultAdminEmail,
                    Email = defaultAdminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(admin, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "SuperAdmin");
                }


            }
        }
    }
}
