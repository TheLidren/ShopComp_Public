using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopComp.Data;
using ShopComp.Models;
using ShopComp.Services;
using System.Collections.Generic;
using System.Linq;

namespace ShopComp.Controllers
{
    public class OrderDetailsController : Controller
    {
        AppDBContent db;
        private UserManager<User> _userManager;
        IWebHostEnvironment _appEnvironment;
        EmailService emailService = new();
        FileService fileService = new();
        OrderDetails orderDetails;
        List<Cart> carts;

        public OrderDetailsController(UserManager<User> userManager, AppDBContent context, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            db = context;
            _appEnvironment = appEnvironment;
        }

        public IActionResult ConfirmOrders() => View(db.OrderDetails.Include(u => u.Users).ToList().Where(p => !p.Submitted && !p.Cancel && !p.Condition));

        public IActionResult SentOrders() => View(db.OrderDetails.Include(u => u.Users).ToList().Where(p => p.Submitted && !p.Cancel && !p.Condition));

        public IActionResult CompletedOrder() => View(db.OrderDetails.Include(u => u.Users).ToList().Where(p => p.Submitted && p.Condition));

        [HttpPost]
        public IActionResult ConfirmReceiptOrder(int id)
        {
            orderDetails = db.OrderDetails.Where(p => p.Id == id).FirstOrDefault();
            orderDetails.Condition = true;
            carts = db.Carts.Include(p => p.Tovar).Where(p => p.OrderDetailsID == id && p.Status).ToList();
            db.OrderDetails.Update(orderDetails);
            carts.ForEach(p => p.Condition = true);
            carts.ForEach(p => p.Tovar.Sold += p.Quantity);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public IActionResult SentReview(OrderDetails model)
        {
            orderDetails = db.OrderDetails.Where(p => p.Id == model.Id).FirstOrDefault();
            orderDetails.Review = model.Review;
            db.OrderDetails.Update(orderDetails);
            db.SaveChanges();
            return RedirectToAction("ListOrder", "Cart");
        }

        [HttpPost]
        public IActionResult CancelOrder(int id)
        {
            orderDetails = db.OrderDetails.Where(p => p.Id == id).FirstOrDefault();
            orderDetails.Cancel = true;
            orderDetails.Condition = true;
            carts = db.Carts.Include(p => p.Tovar).Where(p => p.OrderDetailsID == id && p.Status).ToList();
            carts.ForEach(m => m.Tovar.Amount += m.Quantity);
            db.OrderDetails.Update(orderDetails);
            db.Carts.RemoveRange(carts);
            db.SaveChanges();
            return View();
        }

        [HttpPost]
        public IActionResult DeleteOrder(int? id)
        {
            orderDetails = db.OrderDetails.Find(id);
            db.OrderDetails.Remove(orderDetails);
            db.Carts.RemoveRange(db.Carts.Where(p => p.OrderDetailsID == id && p.Status && p.Condition));
            db.SaveChanges();
            return RedirectToAction("SentOrders");
        }

        [HttpPost]
        public IActionResult ConfirmSentOrder(int? id)
        {
            orderDetails = db.OrderDetails.Include(p => p.Users).Where(p => p.Id == id).FirstOrDefault();
            orderDetails.Submitted = true;
            carts = db.Carts.Include(x => x.Tovar).Where(p => p.OrderDetailsID == id && p.Status).ToList();
            carts.ForEach(m => m.Submitted = true);
            db.OrderDetails.Update(orderDetails);
            emailService.SendEmailAsync(orderDetails.Users.Email, "Заказ отправлен",
            $"<h2>Заказ № {orderDetails.Id}</h2><h4>Ваша посылка была отправлена. Ориентировочное время доставки 3 дня.</h4>");
            db.SaveChanges();
            return RedirectToAction("ConfirmOrders");
        }

        [HttpGet]
        public ActionResult MoreDetails(int? id)
        {
            if (id == null)
                return NotFound("Страница не найдена");
            orderDetails = db.OrderDetails.Include(p => p.Users).Where(p => p.Id == id).FirstOrDefault();
            ViewBag.User = orderDetails.Users.Name + " " + orderDetails.Users.Surname + " " + orderDetails.Users.Patronymic;
            ViewBag.Phone = orderDetails.Phone;
            ViewBag.Country = orderDetails.Country;
            ViewBag.CreditCard = orderDetails.CreditCard;
            ViewBag.Line1 = orderDetails.Line1;
            ViewBag.Line2 = orderDetails.Line2;
            ViewBag.GiftWrap = orderDetails.GiftWrap;
            ViewBag.Submitted = orderDetails.Submitted;
            ViewBag.Condition = orderDetails.Condition;
            carts = db.Carts.Include(x => x.Tovar).Include(x => x.Tovar.Categories).Where(p => p.OrderDetailsID == id && p.Status).ToList();
            if (carts != null)
                return View(carts);
            return NotFound("Страница не найдена");
        }


    }
}
