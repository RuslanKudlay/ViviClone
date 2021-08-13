using System.ComponentModel.DataAnnotations;

namespace Application.EntitiesModels.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public virtual string Password { get ; set; }

        [Display(Name = "Запомнить меня")]
        public virtual bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }       
    }
}