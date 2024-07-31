using Microsoft.AspNetCore.Mvc.Rendering;
using ShopComp.Models;
using System.Collections.Generic;

namespace ShopComp.ViewModels.Tovars
{
    public class FilterViewModel
    {
        public FilterViewModel(List<Category> categories, int? category, string name)
        {
            // устанавливаем начальный элемент, который позволит выбрать всех
            categories.Insert(0, new Category { Tittle = "Все", Id = 0 });
            Categories = new SelectList(categories, "Id", "Name", category);
            SelectedCategory = category;
            SelectedTittle = name;
        }
        public SelectList Categories { get; private set; }
        public int? SelectedCategory { get; private set; }
        public string SelectedTittle { get; private set; }
    }
}

