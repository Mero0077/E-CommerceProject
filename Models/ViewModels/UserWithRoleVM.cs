using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_CommerceFIdentityScaff.Models.ViewModels
{
    public class UserWithRoleVM
    {
        public ApplicationUser User { get; set; }
        public string Role { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }
    }

}
