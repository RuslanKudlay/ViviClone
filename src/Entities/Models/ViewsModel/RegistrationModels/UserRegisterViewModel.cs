using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace Application.EntitiesModels.Models.ViewsModel.RegistrationModels
{
    public class UserRegisterViewModel : RegisterViewModel
    {
        [Required(ErrorMessage = "Введите свой пароль")]
        [StringLength(24, ErrorMessage = "Пароль должен быть от 8 до 24 символов", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string ConfirmPassword { get; set; }

    }
}
