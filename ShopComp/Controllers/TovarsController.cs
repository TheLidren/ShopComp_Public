using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComp.Data;
using ShopComp.Models;
using ShopComp.ViewModels;
using ShopComp.ViewModels.Pages;
using ShopComp.ViewModels.Tovars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopComp.Controllers
{
    public class TovarsController : Controller
    {
        AppDBContent db;
        Tovar tovar;

        public TovarsController(AppDBContent context)
        {
            db = context;
        }

        public async Task<IActionResult> ListTovars(SortState sortOrder = SortState.TittleAsc)
        {
            IQueryable<Tovar> tovars = db.Tovars.Include(u => u.Categories);
            ViewData["TittleSort"] = sortOrder == SortState.TittleAsc ? SortState.TittleDesc : SortState.TittleAsc;
            ViewData["CategorySort"] = sortOrder == SortState.CategoryAsc ? SortState.CategoryDesc : SortState.CategoryAsc;
            ViewData["ColSort"] = sortOrder == SortState.ColAsc ? SortState.ColDesc : SortState.ColAsc;
            ViewData["PriceSort"] = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            tovars = sortOrder switch
            {
                SortState.TittleDesc => tovars.OrderByDescending(s => s.Tittle),
                SortState.ColAsc => tovars.OrderBy(s => s.Amount),
                SortState.ColDesc => tovars.OrderByDescending(s => s.Amount),
                SortState.PriceAsc => tovars.OrderBy(s => s.Price),
                SortState.PriceDesc => tovars.OrderByDescending(s => s.Price),
                SortState.CategoryAsc => tovars.OrderBy(s => s.Categories.Tittle),
                SortState.CategoryDesc => tovars.OrderByDescending(s => s.Categories.Tittle),
                _ => tovars.OrderBy(s => s.Tittle),
            };
            return View(await tovars.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> List(int? category, string name, int minPrice, int sort = 1, int page = 1, int maxPrice = 100000)
        {
            int pageSize = 4;
            IQueryable<Tovar> tovars = db.Tovars.Include(p => p.Categories);
            if (category != null && category != 0)
                tovars = tovars.Where(p => p.CategoriesId == category);
            if (sort != 0)
                if (sort == 1)
                    tovars = tovars.OrderBy(p => p.Price);
                else if (sort == 2)
                    tovars = tovars.OrderByDescending(p => p.Price);
                else if (sort == 3)
                    tovars = tovars.OrderByDescending(p => p.Sold);
            if (minPrice != 0 || maxPrice != 0)
                tovars = tovars.Where(p => p.Price >= minPrice && p.Price <= maxPrice);
            if (!String.IsNullOrEmpty(name))
                tovars = tovars.Where(p => p.Tittle.Contains(name));
            List<Category> categories = db.Categories.ToList();
            categories.Insert(0, new Category { Tittle = "Все", Id = 0 });
            var count = await tovars.CountAsync();
            var items = await tovars.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            IndexViewModel viewModel = new()
            {
                PageViewModel = new PageViewModel(count, page, pageSize),
                FilterViewModel = new FilterViewModel(db.Categories.ToList(), category, name),
                Tovars = items
            };
            ViewBag.Name = name;
            ViewBag.Sort = sort;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            if (category != null)
            {
                ViewBag.Category = db.Categories.ToList().Where(p => p.Id != category && !p.Status);
                ViewBag.CategoryTittle = db.Categories.ToList().Where(p => p.Id == category && !p.Status);
            }
            else ViewBag.Category = db.Categories.ToList().Where(p => !p.Status);
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult AddTovar() => View(db.Categories.ToList().Where(p => !p.Status));

        [HttpPost]
        public IActionResult AddTovar(Tovar order)
        {
            db.Tovars.Add(order);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public async Task<IActionResult> EditTovar(int? id)
        {
            if (id != null)
            {
                tovar = await db.Tovars.FirstOrDefaultAsync(p => p.Id == id);
                if (tovar != null)
                    ViewBag.Category = db.Categories.ToList().Where(p => p.Id != tovar.CategoriesId && !p.Status);
                ViewBag.CategoryTittle = db.Categories.ToList().Where(p => p.Id == tovar.CategoriesId && !p.Status);
                return View(tovar);
            }
            return NotFound("Страница не найдена");
        }

        [HttpPost]
        public ActionResult EditTovar(Tovar tovar)
        {
            db.Tovars.Update(tovar);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public ActionResult MoreDetails(int? id)
        {
            if (id == null)
                return NotFound("Страница не найдена");
            tovar = db.Tovars.Find(id);
            ViewBag.CategoryTittle = db.Categories.Where(p => p.Id == tovar.CategoriesId && !p.Status);
            if (tovar != null)
                return View(tovar);
            return NotFound("Страница не найдена");
        }

        [HttpPost]
        public ActionResult DeleteTovar(int? id)
        {
            Cart cart = db.Carts.Where(p => p.Tovar.Id == id).FirstOrDefault();
            if (cart != null)
                db.Remove(cart);
            tovar = db.Tovars
                .FirstOrDefault(c => c.Id == id);
            if (tovar != null)
                db.Tovars.Remove(tovar);
            db.SaveChanges();
            return RedirectToAction("List");
        }
    }
}

