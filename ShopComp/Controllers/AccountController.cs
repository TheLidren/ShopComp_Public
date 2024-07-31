using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShopComp.Models;
using ShopComp.Services;
using ShopComp.ViewModels.User;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Threading.Tasks;

namespace ShopComp.Controllers
{
    public class AccountController : Controller
    {

        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        IWebHostEnvironment _appEnvironment;
        FileService fileService = new();
        EmailService emailService = new();
        User user;
        string str;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IWebHostEnvironment appEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index() => View(_userManager.Users.ToList());

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email.Contains("@gmail.ru") || model.Email.Contains("@mail.com"))
                {
                    ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                    return View(model);
                }
                user = new() { Email = model.Email, UserName = model.Email, Name = model.Name, Surname = model.Surname, Patronymic = model.Patronymic };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "user");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, Code = code },
                        protocol: HttpContext.Request.Scheme);
                    if (model.Email.Contains("@gmail.com"))
                        emailService.SendEmailDefault(model.Email, "Подтвердите ваш Email.",
                            $"Подтвердите регистрацию, перейдя по ссылке и войдите в свою учётную запись: <a href='{callbackUrl}'>Ссылка</a>");
                    else if (model.Email.Contains("@mail.ru"))
                        emailService.SendEmailAsync(model.Email, "Подтвердите ваш Email.",
                            $"Подтвердите регистрацию, перейдя по ссылке и войдите в свою учётную запись: <a href='{callbackUrl}'>Ссылка</a>");
                    return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                else
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
                return View("Error");
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return View("Error");
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
                return RedirectToAction("Login", "Account");
            else
                return View("Error");
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null) => View(new LoginViewModel { ReturnUrl = returnUrl });

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email.Contains("@gmail.ru") || model.Email.Contains("@mail.com"))
                {
                    ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                    return View(model);
                }
                using (var sr = new StreamWriter(new IsolatedStorageFileStream(_appEnvironment.WebRootPath + "\\file.txt", FileMode.Truncate)))
                {
                    sr.WriteLine(model.Email);
                    sr.Close();
                }
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "Вы не подтвердили свой email");
                        return View(model);
                    }
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                    return RedirectToAction("List", "Tovars");
                else
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("List", "Tovars");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword() => View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Email.Contains("@gmail.ru") || model.Email.Contains("@mail.com"))
                {
                    ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                    return View(model);
                }
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    using (var sr = new StreamWriter(new IsolatedStorageFileStream(_appEnvironment.WebRootPath + "\\file.txt", FileMode.Truncate)))
                    {
                        sr.WriteLine(model.Email);
                        sr.Close();
                    }
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code }, protocol: HttpContext.Request.Scheme);
                    EmailService emailService = new();
                    emailService.SendEmailAsync(model.Email, "Reset Password",
                        $"Для сброса пароля пройдите по ссылке: <a href='{callbackUrl}'>link</a>");
                    return View("ForgotPasswordConfirmation");
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null) => code == null ? View("Error") : View();

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                str = fileService.EmailUser(_appEnvironment);
                user = await _userManager.FindByEmailAsync(str);
                if (user == null)
                    return View("ResetPasswordConfirmation");
                var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
                if (result.Succeeded)
                    return View("ResetPasswordConfirmation");
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser()
        {
            str = fileService.EmailUser(_appEnvironment);
            user = await _userManager.FindByEmailAsync(str);
            if (user == null)
                return NotFound();
            EditUserViewModel model = new() { Id = user.Id, Name = user.Name, Surname = user.Surname, Patronymic = user.Patronymic };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.Patronymic = model.Patronymic;
                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("List", "Tovars");
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(string id)
        {
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            await _signInManager.SignOutAsync();
            await _userManager.DeleteAsync(user);
            return RedirectToAction("List", "Tovars");
        }

        public async Task<IActionResult> ChangePassword(string id)
        {
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            ChangePasswordViewModel model = new() { Id = user.Id, Email = user.Email };
            return View(model);
        }

        public async Task<IActionResult> ChangeLogin(string id)
        {
            user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();
            ChangeLoginViewModel model = new() { Id = user.Id, Email = user.Email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeLogin(ChangeLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.NewLogin.Contains("@gmail.ru") || model.NewLogin.Contains("@mail.com"))
                {
                    ModelState.AddModelError("Email", "Некорректный адрес. Только mail.ru или gmail.com");
                    return View(model);
                }
                user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    using (var sr = new StreamWriter(new IsolatedStorageFileStream(_appEnvironment.WebRootPath + "\\file.txt", FileMode.Truncate)))
                    {
                        sr.WriteLine(model.NewLogin);
                        sr.Close();
                    }
                    user.Email = model.NewLogin;
                    user.UserName = model.NewLogin;
                    user.EmailConfirmed = false;
                    await _signInManager.SignInAsync(user, false);
                    await _userManager.AddToRoleAsync(user, "user");
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "Account",
                        new { userId = user.Id, Code = code },
                        protocol: HttpContext.Request.Scheme);
                    if (model.NewLogin.Contains("@gmail.com"))
                        emailService.SendEmailDefault(model.NewLogin, "Подтвердите ваш Email.",
                            $"Подтвердите регистрацию, перейдя по ссылке и войдите в свою учётную запись: <a href='{callbackUrl}'>Ссылка</a>");
                    else if (model.NewLogin.Contains("@mail.ru"))
                        emailService.SendEmailAsync(model.NewLogin, "Подтвердите ваш Email.",
                            $"Подтвердите регистрацию, перейдя по ссылке и войдите в свою учётную запись: <a href='{callbackUrl}'>Ссылка</a>");
                    return Content("Для завершения изменения пароля проверьте электронную почту и перейдите по ссылке, указанной в письме");
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                user = await _userManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    IdentityResult result =
                        await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                        return RedirectToAction("List", "Tovars");
                    else
                        foreach (var error in result.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);
                }
                else
                    ModelState.AddModelError(string.Empty, "Пользователь не найден");
            }
            return View(model);
        }

    }
}
