using System.ComponentModel.DataAnnotations;

namespace Application.EntitiesModels.Models.Auth
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
