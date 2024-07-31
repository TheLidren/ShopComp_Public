using System.ComponentModel.DataAnnotations;

namespace ShopComp.ViewModels.User
{
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        //[RegularExpression(@"[а-яА-ЯёЁa-zA-Z0-9]+$", ErrorMessage = "Некорректный пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароль должна быть от 8 до 50 символов")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Длина пароль должна быть от 8 до 50 символов")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
    }
}
