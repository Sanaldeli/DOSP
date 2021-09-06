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
        private readonly Model m = new Model();
        [HttpGet]
        public ActionResult Index()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Magaza");
            }
            ViewBag.kullanici = m.Kullanicis.ToList();
            ViewBag.yapimci = m.Yapimcis.ToList();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(Kullanici k, string ReturnUrl)
        {
            Kullanici tmpk = m.Kullanicis.FirstOrDefault(x => x.rumuz == k.rumuz && x.sifre == k.sifre);
            if (tmpk != null)
            {
                FormsAuthentication.SetAuthCookie(tmpk.rumuz, false);
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
        public ActionResult Kayit(Kullanici k)
        {
            if (ModelState.IsValid)
            {
                using (DOSPEntities db = new DOSPEntities())
                {
                    var check = db.Kullanicis.FirstOrDefault(s => s.rumuz == k.rumuz);
                    if (check == null)
                    {
                        k.kayitTarihi = DateTime.Now;
                        db.Configuration.ValidateOnSaveEnabled = false;
                        k.Rol = "K";
                        db.Kullanicis.Add(k);
                        Sepet s = new Sepet
                        {
                            KullaniciID = k.KullaniciID
                        };
                        db.Sepets.Add(s);
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