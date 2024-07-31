using ShopComp.Models;
using System.Linq;

namespace ShopComp.Data
{
    public class InsertCompDB
    {

        public static void Initialize(AppDBContent context)
        {
            Category category1 = null, category3 = null;
            if (!context.Categories.Any())
            {
                category1 = new() { Tittle = "Ноутбуки", Desc = "Компактный портативный компьютер" };
                Category category2 = new() { Tittle = "Компьютеры", Desc = "Электронное устройство для работы с информацией и данными" };
                category3 = new() { Tittle = "Перефирийные устройства", Desc = "Устройства, которые подключаются к ПК для расширения его функциональных возможностей" };
                Category category4 = new() { Tittle = "Аксессуары к ноутбукам и компьютерам", Desc = "Устройства, которые можно использовать с компьютерами или ноутбуками" };
                context.Categories.AddRange(category1, category2, category3, category4);
                context.SaveChanges();
            }
            if (!context.Tovars.Any())
            {
                context.Tovars.AddRange(
                    new Tovar
                    {
                        Tittle = "Lenovo Legion Y530",
                        ShortDesc = "14.0d 1920 x 1080 IPS, 60 Гц, несенсорный, Intel Core i5 1135G7 2400 МГц, 8 ГБ, SSD 512 ГБ, граф. адаптер: встроенная, Windows 10, цвет крышки серый",
                        LongDesc = "14.0d 1920 x 1080 IPS, 60 Гц, несенсорный, Intel Core i5 1135G7 2400 МГц, 8 ГБ, SSD 512 ГБ, граф. адаптер: встроенная, Windows 10, цвет крышки серый",
                        Img = "https://cdn21vek.by/img/galleries/6030/317/preview_b/tufgamingfx505dvhn279_asus_5f2bac86ab41b.jpeg",
                        Price = 2000,
                        CategoriesId = category1.Id,
                        Amount = 30
                    },
                    new Tovar
                    {
                        Tittle = "HONOR MagicBook X15",
                        ShortDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, Intel Core i3 10110U 2100 МГц, 8 ГБ, SSD 256 ГБ, видеокарта встроенная, Windows 10, цвет крышки серый",
                        LongDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, Intel Core i3 10110U 2100 МГц, 8 ГБ, SSD 256 ГБ, видеокарта встроенная, Windows 10, цвет крышки серый",
                        Img = "https://content2.onliner.by/catalog/device/main/02b46d2a97dbc19011f957807a23963d.jpeg",
                        Price = 1500,
                        CategoriesId = category1.Id,
                        Amount = 50
                    },
                    new Tovar
                    {
                        Tittle = "Lenovo IdeaPad Gaming 3",
                        ShortDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, AMD Ryzen 5 4600H 3000 МГц, 8 ГБ, SSD 512 ГБ, видеокарта NVIDIA GeForce GTX 1650 4 ГБ GDDR6, без ОС, цвет крышки черный",
                        LongDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, AMD Ryzen 5 4600H 3000 МГц, 8 ГБ, SSD 512 ГБ, видеокарта NVIDIA GeForce GTX 1650 4 ГБ GDDR6, без ОС, цвет крышки черный",
                        Img = "https://content2.onliner.by/catalog/device/main/4904e24d7e36e19818c954179881d5dc.jpeg",
                        Price = 3000,
                        CategoriesId = category1.Id
                    },
                    new Tovar
                    {
                        Tittle = "HP 15s-eq2021ur",
                        ShortDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, AMD Ryzen 5 5500U 2100 МГц, 16 ГБ, SSD 512 ГБ, видеокарта встроенная, без ОС, цвет крышки серебристый",
                        LongDesc = "15.6d 1920 x 1080 IPS, 60 Гц, несенсорный, AMD Ryzen 5 5500U 2100 МГц, 16 ГБ, SSD 512 ГБ, видеокарта встроенная, без ОС, цвет крышки серебристый",
                        Img = "https://content2.onliner.by/catalog/device/main/e1c23dbe3d11f1071484573d63a9311c.jpeg",
                        Price = 1000,
                        CategoriesId = category1.Id
                    },
                    new Tovar
                    {
                        Tittle = "Fury Hustler",
                        ShortDesc = "полноразмерная игровая мышь для ПК, проводная USB, сенсор оптический 6400 dpi, 7 кнопок, колесо с нажатием, цвет черный",
                        LongDesc = "полноразмерная игровая мышь для ПК, проводная USB, сенсор оптический 6400 dpi, 7 кнопок, колесо с нажатием, цвет черный",
                        Img = "https://ichip.ru/images/cache/2019/4/13/fit_930_519_false_crop_1143_743_0_0_q90_340212_2e7c9774df.jpeg",
                        Price = 30,
                        CategoriesId = category3.Id,
                        Amount = 70
                    },
                    new Tovar
                    {
                        Tittle = "SteelSeries Prime",
                        ShortDesc = "полноразмерная игровая мышь для ПК/для Microsoft Xbox/для компьютеров Apple, проводная USB, сенсор оптический 18000 dpi, 6 кнопок, колесо с нажатием, цвет черный",
                        LongDesc = "полноразмерная игровая мышь для ПК/для Microsoft Xbox/для компьютеров Apple, проводная USB, сенсор оптический 18000 dpi, 6 кнопок, колесо с нажатием, цвет черный",
                        Img = "https://i.pcmag.com/imagery/reviews/02lwE6xbBiSCBqENotwO4gh-1.1621526817.fit_scale.size_760x427.jpg",
                        Price = 1000,
                        CategoriesId = category3.Id
                    });
                context.SaveChanges();
            }
        }
    }
}


