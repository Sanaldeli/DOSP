using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOSP.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DOSP.Controllers
{
    public class GirisController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Magaza");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User k, string ReturnUrl)
        {
            User user = _dc.Users.FirstOrDefault(x => x.Nickname == k.Nickname);
            if (user is null || !BCryptNet.Verify(k.Password, user.Password))
            {
                ViewBag.error = "Kullanıcı adı veya parola hatalı.";
                return View();
            }

            FormsAuthentication.SetAuthCookie(user.Nickname, false);
            if (!string.IsNullOrEmpty(ReturnUrl))
                return Redirect(ReturnUrl);

            return RedirectToAction("Index", "Magaza");
        }

        [HttpGet]
        public ActionResult Kayit()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Magaza");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kayit(User k)
        {
            if (!ModelState.IsValid)
                return View();

            if (_dc.Users.Any(u => u.Nickname == k.Nickname))
            {
                ViewBag.error = "Bu kullanıcı adı daha önce alınmış";
                return View();
            }

            k.Password = BCryptNet.HashPassword(inputKey: k.Password, workFactor: 12);
            k.Role = "K";
            k.ProfilePicture = "default-pp.jpg";

            _dc.Configuration.ValidateOnSaveEnabled = false;
            _dc.Users.Add(k);

            Basket s = new Basket
            {
                UserID = k.ID
            };
            _dc.Baskets.Add(s);

            _dc.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}