using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DOSP.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            List<User> k = _dc.Users.ToList();
            return View(k);
        }

        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            var data = _dc.Users.Where(x => x.ID == id).SingleOrDefault();
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(User k, string CandidatePassword, bool admin, bool yapimci)
        {
            var user = _dc.Users.First(x => x.ID == k.ID);

            if (user is null)
            {
                ViewBag.error = "Kullanıcı bulunamadı.";
                return View();
            }

            user.Nickname = k.Nickname;
            user.FullName = k.FullName;
            user.Wallet = k.Wallet;

            if (!string.IsNullOrEmpty(CandidatePassword))
                user.Password = BCryptNet.HashPassword(k.Password, workFactor: 12);

            string userRoles = "K";
            if (admin)
            {
                userRoles += "A";
            }
            if (yapimci)
            {
                userRoles += "Y";
            }
            user.Role = userRoles;

            _dc.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            User k = _dc.Users.Find(id);
            _dc.Users.Remove(k);

            _dc.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}