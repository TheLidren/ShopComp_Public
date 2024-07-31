using Microsoft.AspNetCore.Mvc;
using ShopComp.Data;
using ShopComp.Models;
using System.Linq;

namespace ShopComp.Controllers
{
    public class CategoryController : Controller
    {
        AppDBContent db;
        Category category;

        public CategoryController(AppDBContent context)
        {
            db = context;
        }

        public IActionResult ListCategory() => View(db.Categories.ToList().Where(p => !p.Status));

        [HttpGet]
        //[Authorize(Roles = "admin")]
        public IActionResult AddCategory() => View();

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            db.Categories.Add(category);
            db.SaveChanges();
            return RedirectToAction("List");
        }

        [HttpGet]
        public IActionResult EditCategory(int? id)
        {
            if (id != null)
            {
                category = db.Categories.Where(p => p.Id == id).FirstOrDefault();
                if (category != null)
                    return View(category);
            }
            return NotFound("Страница не найдена");
        }

        [HttpPost]
        public ActionResult EditCategory(Category category)
        {
            db.Categories.Update(category);
            db.SaveChanges();
            return RedirectToAction("ListCategory", "Category");
        }

        [HttpPost]
        public ActionResult DeleteCategory(int? id)
        {
            category = db.Categories
                .FirstOrDefault(c => c.Id == id);
            category.Status = true;
            if (category != null)
                db.Categories.Update(category);
            db.SaveChanges();
            return RedirectToAction("ListCategory", "Category");
        }

    }
}
