using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

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
            using (var context = new DataContext())
            {
                var data = context.Users.Where(x => x.ID == id).SingleOrDefault();
                return View(data);
            }
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(User k, bool admin, bool yapimci)
        {
            using (var context = new DataContext())
            {
                var data = context.Users.FirstOrDefault(x => x.ID == k.ID);

                if (data != null)
                {
                    data.Nickname = k.Nickname;
                    data.FullName = k.FullName;
                    data.Wallet = k.Wallet;
                    data.Password = k.Password;

                    string roller = "K";
                    if (admin)
                    {
                        roller += "A";
                    }
                    if (yapimci)
                    {
                        roller += "Y";
                    }
                    data.Role = roller;

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            User k = _dc.Users.FirstOrDefault(x => x.ID == id);
            _dc.Users.Remove(k);
            _dc.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}