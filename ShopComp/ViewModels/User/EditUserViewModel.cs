using System.ComponentModel.DataAnnotations;

namespace ShopComp.ViewModels.User
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

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

        [Required(ErrorMessage = "Не указана пароль")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароль должна быть от 8 до 50 символов")]
        public string Password { get; set; }
    }
}
