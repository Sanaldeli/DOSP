using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class GirisController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                Console.WriteLine(HttpContext.User.Identity);
                return RedirectToAction("Index", "Magaza");
            }

            ViewBag.kullanici = _dc.Users.ToList();
            ViewBag.yapimci = _dc.Developers.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User k, string ReturnUrl)
        {
            User tmpk = _dc.Users.FirstOrDefault(x => x.Nickname == k.Nickname && x.Password == k.Password);
            if (tmpk != null)
            {
                FormsAuthentication.SetAuthCookie(tmpk.Nickname, false);
                if (!string.IsNullOrEmpty(ReturnUrl))
                {
                    return Redirect(ReturnUrl);
                }
                return RedirectToAction("Index", "Magaza");
            }
            ViewBag.error = "Kullanıcı adı veya parola hatalı.";
            return View();
        }

        public ActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Kayit(User k)
        {
            if (ModelState.IsValid)
            {
                using (DataContext db = new DataContext())
                {
                    var check = db.Users.FirstOrDefault(s => s.Nickname == k.Nickname);
                    if (check == null)
                    {
                        k.RegistrationDate = DateTime.Now;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        k.Role = "K";
                        db.Users.Add(k);
                        Basket s = new Basket
                        {
                            UserID = k.ID
                        };
                        db.Baskets.Add(s);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    ViewBag.error = "Bu rumuz daha önce alınmış";
                    return View();
                }
            }
            return View();
        }

        [Authorize]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }
    }
}