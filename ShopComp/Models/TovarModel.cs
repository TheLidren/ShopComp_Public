using System.Collections.Generic;
using System.Linq;

namespace ShopComp.Models
{
    public class TovarModel
    {
        public List<Tovar> Tovars { get; set; }

        public Tovar Find(int id)
        {
            var prop = Tovars.Where(p => p.Id == id).FirstOrDefault();
            prop.Amount--;
            return prop;
        }

    }
}
