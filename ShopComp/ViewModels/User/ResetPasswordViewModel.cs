using System.ComponentModel.DataAnnotations;

namespace ShopComp.ViewModels.User
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Не указан email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[mail|gmail]*\.(ru|com)$", ErrorMessage = "Некорректный адрес. Только mail.ru или gmail.com")]
        [StringLength(100, ErrorMessage = "Длина строки должна быть до 100 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указана пароль")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 8 символов", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указана пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }

    }
}
