using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ShopComp.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указано название")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 100 символов")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z]+$", ErrorMessage = "Некорректное название")]
        public string Tittle { get; set; }

        [Required(ErrorMessage = "Не указано описание")]
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Длина строки должна быть от 3 до 500 символов")]
        [RegularExpression(@"[а-яА-ЯёЁa-zA-Z0-9]+$", ErrorMessage = "Некорректное описание")]
        public string Desc { get; set; }

        public bool Status { get; set; }

        public List<Tovar> Tovars { get; set; } = new List<Tovar>();
    }
}
