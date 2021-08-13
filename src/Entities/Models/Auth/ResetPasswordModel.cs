using System.ComponentModel.DataAnnotations;

namespace Application.EntitiesModels.Models.Auth
{
    public class ResetPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Код сброса пароля")]
        public string Code { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 8 символов", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }
    }
}
