using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComp.Data;
using ShopComp.Models;
using ShopComp.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShopComp.Controllers
{
    public class CartController : Controller
    {
        AppDBContent db;
        IWebHostEnvironment _appEnvironment;
        EmailService emailService = new();
        FileService fileService = new();
        string str;

        public CartController(AppDBContent context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public void BuyTovar(int id)
        {
            string str = fileService.EmailUser(_appEnvironment);
            Cart b = db.Carts.Where(p => p.Tovar.Id == id && p.Users.Email == str && !p.Status).FirstOrDefault();
            User user = db.Users.Where(p => p.Email == str).FirstOrDefault();
            TovarModel productModel = new();
            productModel.Tovars = db.Tovars.ToList();
            if (b == null)
            {
                List<Cart> cart = new();
                cart.Add(new Cart { Tovar = productModel.Find(id), Quantity = 1, Users = user, Status = false });
                db.Carts.Add(cart[0]);
                db.SaveChanges();
            }
            else
            {
                b.Quantity++;
                productModel.Find(id);
                db.SaveChanges();
            }
        }

        public IActionResult Index()
        {
            string str = fileService.EmailUser(_appEnvironment);
            List<Cart> carts = db.Carts.Include(x => x.Tovar).Where(p => p.Users.Email == str && !p.Status && !p.Condition).ToList();
            ViewBag.total = carts.Sum(item => item.Tovar.Price * item.Quantity);
            return View(carts);
        }

        public IActionResult ListOrder()
        {
            string str = fileService.EmailUser(_appEnvironment);
            List<Cart> carts = db.Carts.Include(x => x.Tovar).Where(p => p.Users.Email == str && p.Status && !p.Condition).ToList();
            ViewBag.total = carts.Sum(item => item.Tovar.Price * item.Quantity);
            ViewBag.OrderDetailsId = db.Carts.Where(p => p.Users.Email == str && p.Status && !p.Condition).ToList();
            return View(carts);
        }

        public IActionResult Buy(int id)
        {
            BuyTovar(id);
            return RedirectToAction("List", "Tovars");
        }

        public IActionResult BuyCart(int id)
        {
            BuyTovar(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Buy(int id, int count)
        {
            string str = fileService.EmailUser(_appEnvironment);
            Cart b = db.Carts.Where(p => p.Tovar.Id == id && p.Users.Email == str && !p.Status).FirstOrDefault();
            User user = db.Users.Where(p => p.Email == str).FirstOrDefault();
            TovarModel productModel = new();
            productModel.Tovars = db.Tovars.ToList();
            var prop = db.Tovars.Where(p => p.Id == id).FirstOrDefault();
            prop.Amount -= count;
            if (b == null)
            {
                List<Cart> cart = new();
                cart.Add(new Cart { Tovar = prop, Quantity = count, Users = user, Status = false });
                db.Carts.Add(cart[0]);
                db.SaveChanges();
            }
            else
            {
                b.Quantity += count;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            Cart b = db.Carts.Where(p => p.Id == id && !p.Condition).Include(p => p.Tovar).FirstOrDefault();
            if (b != null)
            {
                Tovar tovar = db.Tovars.Find(b.Tovar.Id);
                tovar.Amount++;
                b.Quantity--;
                if (b.Quantity <= 0)
                    db.Carts.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            Cart b = db.Carts.Where(p => p.Id == id && !p.Condition).Include(p => p.Tovar).FirstOrDefault();
            if (b != null)
            {
                Tovar tovar = db.Tovars.Find(b.Tovar.Id);
                tovar.Amount += b.Quantity;
                db.Carts.Remove(b);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Checkout(Cart cart)
        {
            string str = fileService.EmailUser(_appEnvironment);
            List<Cart> carts = db.Carts.Include(x => x.Tovar).Where(p => p.Users.Email == str && !p.Condition).ToList();
            if (carts.Count == 0)
                return RedirectToAction("Error");
            else return View();
        }

        [HttpPost]
        public IActionResult Checkout(OrderDetails orderDetails)
        {
            if (ModelState.IsValid)
            {
                str = fileService.EmailUser(_appEnvironment);
                User user = db.Users.Where(p => p.Email == str).FirstOrDefault();
                List<Cart> carts = db.Carts.Include(x => x.Tovar).Where(p => p.Users.Email == str && !p.Status && !p.Condition).ToList();
                orderDetails.UsersId = user.Id;
                orderDetails.Users = user;
                orderDetails.Price = carts.Sum(item => item.Tovar.Price * item.Quantity);
                db.OrderDetails.Add(orderDetails);
                db.SaveChanges();
                emailService.SendEmailOrder(str, carts, orderDetails);
                carts.ForEach(m => m.Status = true);
                carts.ForEach(m => m.OrderDetailsID = orderDetails.Id);
                db.SaveChanges();
                return RedirectToAction("Completed", "Cart");
            }
            else return View(orderDetails);
        }

        public ViewResult Completed() => View();

        public ViewResult Error() => View();

    }
}
