using Application.EntitiesModels.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Application.EntitiesModels.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //Test
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public UserStatus Status { get; set; }

        public IEnumerable<string> Logins { get; set; }
    }
}
