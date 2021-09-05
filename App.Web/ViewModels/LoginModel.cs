using System.ComponentModel.DataAnnotations;

namespace App.Web.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Введите логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Введите пароль")]
        public string Password { get; set; }
    }
}
