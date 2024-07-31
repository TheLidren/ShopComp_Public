using ShopComp.Models;
using ShopComp.ViewModels.Tovars;
using System.Collections.Generic;

namespace ShopComp.ViewModels.Pages
{
    public class IndexViewModel
    {
        public IEnumerable<Tovar> Tovars { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public FilterViewModel FilterViewModel { get; set; }
    }
}
