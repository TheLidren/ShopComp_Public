using ShopComp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ShopComp.Services
{
    public class EmailService
    {
        public bool WriteAsFile = true;
        MailMessage mail;

        public void SendEmailDefault(string email, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress("vladmisevic92@gmail.com", "Подтвердите регистрацию");
                message.To.Add(email);
                message.Subject = subject;
                message.Body = body;
                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    client.Credentials = new NetworkCredential("vladmisevic92@gmail.com", "EfT41DjGK5xNT57Zn1AVM");
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException().Message);
            }
        }

        public void SendEmailAsync(string email, string subject, string message)
        {
            mail = new();
            mail.From = new MailAddress("the_lidrenvlamis@mail.ru");
            mail.To.Add(email);
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true; // можно поставить false, если отправляется только текст
            //mail.Attachments.Add(new Attachment("C:\\File.txt")); // если нужно прикрепить текстовый файл
            //mail.Attachments.Add(new Attachment("C:\\Zip.zip")); // если нужно прикрепить архив
            using SmtpClient smtp = new("smtp.mail.ru", 587);
            smtp.Credentials = new NetworkCredential("the_lidrenvlamis@mail.ru", "qfcd357dbMV");
            smtp.EnableSsl = true;
            smtp.Send(mail);
        }

        public void SendEmailOrder(string email, List<Cart> carts, OrderDetails shippingInfo)
        {
            StringBuilder body = new StringBuilder()
                .AppendLine($"<h2>Ваш заказ с номером {shippingInfo.Id} оформлен</h2>")
                .AppendLine("<h4>Ваши данные:</h4>")
                .AppendFormat("<p><b>ФИО:</b> {0} {1} {2}<p>", shippingInfo.Users.Surname, shippingInfo.Users.Name, shippingInfo.Users.Patronymic)
                .AppendFormat("<p><b>Страна:</b> {0}<p>", shippingInfo.Country)
                .AppendFormat("<p><b>Адрес 1:</b> {0}<p>", shippingInfo.Line1);
            if (shippingInfo.Line2 == "") body.AppendLine("<p><b>Адрес 2:</b> не указан<p>");
            else
                body.AppendFormat("<p><b>Адрес 2:</b> {0}<p>", shippingInfo.Line2)
            .AppendFormat("<p><b>Номер телефона:</b> {0}<p>", shippingInfo.Phone)
            .AppendFormat("<p><b>Расрочка на 6 месяцев: </b>{0}</p>",
                shippingInfo.GiftWrap ? "Да" : "Нет")
            .AppendLine($"<h4>Список товаров:</h4>")
            .AppendLine("<table border='1' cellpadding='4' width='100%'><tr><th>Название</th><th>Количество</th><th>Итоговая цена</th></tr>");
            foreach (var line in carts)
            {
                var subtotal = line.Tovar.Price * line.Quantity;
                body.AppendFormat($"<tr align='center'><td>{line.Tovar.Tittle}</td><td>{line.Quantity}</td><td>{subtotal}</td></tr>");
            }
            body.AppendFormat("<tr><td align='right' colspan='3'>Общая стоимость: {0:c}</td></tr></table><br />", carts.Sum(item => item.Tovar.Price * item.Quantity));
            using SmtpClient smtp = email.Contains("gmail.com") ? new("smtp.gmail.com", 587) : new("smtp.mail.ru", 587);
            smtp.Credentials = email.Contains("gmail.com") ? new NetworkCredential("vladmisevic92@gmail.com", "EfT41DjGK5xNT57Zn1AVM") : new NetworkCredential("the_lidrenvlamis@mail.ru", "qfcd357dbMV");
            smtp.EnableSsl = true;
            mail = new(
                       email.Contains("gmail.com") ? "vladmisevic92@gmail.com" : "the_lidrenvlamis@mail.ru",
                       email,
                       "Ваш заказ оформлен!",
                       body.ToString());
            mail.IsBodyHtml = true;
            if (WriteAsFile)
            {
                mail.BodyEncoding = Encoding.UTF8;
            }
            smtp.Send(mail);
        }
    }
}
