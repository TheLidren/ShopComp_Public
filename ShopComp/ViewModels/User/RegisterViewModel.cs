using System.ComponentModel.DataAnnotations;

namespace ShopComp.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Не указано имя")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Некорректное имя")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Некорректная фамилия")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не указана отчество")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Некорректное отчество")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Не указан email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[mail|gmail]*\.(ru|com)$", ErrorMessage = "Некорректный адрес. Только mail.ru или gmail.com")]
        [StringLength(100, ErrorMessage = "Длина строки должна быть до 100 символов")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указана пароль")]
        [StringLength(50, ErrorMessage = "Пароль должен содержать как от 8 до 50 символов", MinimumLength = 8)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Не указана пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}
