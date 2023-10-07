using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using System.Net.Mail;
using WebApplication3.Models;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private LanguageService _localization;
        public HomeController(ILogger<HomeController> logger, LanguageService localization)
        {
            _logger = logger;
            _localization = localization;
        }

        public IActionResult Index()
        {
            ViewBag.Welcome = _localization.Getkey("Welcome").Value;
            var currentCulture = Thread.CurrentThread.CurrentCulture.Name;
            return View();
        }

        public IActionResult ChangeLanguage(string culture)
        {
            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions()
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult SendMessage(string subject, string namesurname, string number)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse("khanmammad.kh@siesco.az");
            email.To.Add(MailboxAddress.Parse("khanmammad.kh@siesco.az"));
            email.Subject = "Contact(" + subject + "-" + namesurname + "-" + number + ")";
            var builder = new BodyBuilder();
            builder.TextBody = "";
            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("az-s3.ourhost.az", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("khanmammad.kh@siesco.az", "Xanmed2705");
            smtp.Send(email);
            smtp.Disconnect(true);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult about()
        {
            return View();
        }
        public IActionResult accounting()
        {
            return View();
        }
        public IActionResult preference()
        {
            return View();
        }
        public IActionResult partner()
        {
            return View();
        }
        public IActionResult generalservices()
        {
            return View();
        }
        public IActionResult specialservices()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}