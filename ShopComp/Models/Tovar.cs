using System.ComponentModel.DataAnnotations.Schema;

namespace ShopComp.Models
{
    public class Tovar
    {
        public int Id { get; set; }
        public string Img { get; set; }
        public string Tittle { get; set; }
        public string ShortDesc { get; set; }
        public string LongDesc { get; set; }
        public int Price { get; set; }
        public int Amount { get; set; }
        public int Sold { get; set; }
        [ForeignKey("FK_Category")]
        public int CategoriesId { get; set; }
        public Category Categories { get; set; }

    }
}
