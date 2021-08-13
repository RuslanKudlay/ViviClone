using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Application.EntitiesModels.Models.ViewsModel.RegistrationModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите свой email")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите свой телефон")]
        [Phone]
        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите своё имя")]
        [StringLength(100)]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите свою фамилию")]
        [StringLength(100)]
        [Display(Name = "Фамилия")]
        public string SecondName { get; set; }

        [Display(Name = "Запомнить меня")]
        public bool RememberMe { get; set; }
    }
}