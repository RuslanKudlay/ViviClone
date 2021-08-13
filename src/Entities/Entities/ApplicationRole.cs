using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.EntitiesModels.Entities
{
    public class ApplicationRole : IdentityRole<int>
    {
        [StringLength(250)]
        public string Description { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
