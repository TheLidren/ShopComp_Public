using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopComp.Models
{
    public class OrderDetails
    {
        public int Id { get; set; }
        [ForeignKey("FK_User")]
        public string UsersId { get; set; }
        public User Users { get; set; }

        [Required(ErrorMessage = "Укажите адрес доставки")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 500 символов")]
        public string Line1 { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 500 символов")]
        public string Line2 { get; set; }

        public string Review { get; set; }
        public int Price { get; set; }

        [Required(ErrorMessage = "Укажите номер телефона")]
        [RegularExpression(@"^(\+375|80)(29|25|44|33)(\d{3})(\d{2})(\d{2})$", ErrorMessage = "Некорректный номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Укажите страну")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Некорректно указана страна")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 50 символов")]
        public string Country { get; set; }

        [CreditCard(ErrorMessage = "Укажите корректно номер карты")]
        [Required(ErrorMessage = "Укажите номер карты")]
        public string CreditCard { get; set; }

        public bool GiftWrap { get; set; }
        public bool Submitted { get; set; }
        public bool Cancel { get; set; }
        public bool Condition { get; set; }

    }
}
