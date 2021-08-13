using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.EntitiesModels.Entities
{
    public enum UserStatus
    {
        Active,
        Disabled,
        Blocked
    }

    public class ApplicationUser : IdentityUser<int>
    {
        public bool IsEnabled { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
        [StringLength(250)]
        public string FirstName { get; set; }
        [StringLength(250)]
        public string LastName { get; set; }
        [NotMapped]
        public string Name
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }

        public UserStatus Status { get; set; }

        public virtual List<Order.Order> Orders { get; set; }

        public virtual List<Order.OrderHistory> OrderHistories { get; set; }

        public virtual List<WishList.WishList> WishLists { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
