using System.ComponentModel.DataAnnotations;

namespace Application.EntitiesModels.Models.Auth
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
