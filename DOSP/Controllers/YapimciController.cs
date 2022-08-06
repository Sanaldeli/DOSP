using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class YapimciController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize(Roles = "A,Y")]
        public ActionResult Index()
        {
            List<Developer> y = _dc.Developers.ToList();
            return View(y);
        }

        public ActionResult Firma(int? id)
        {
            Developer y;
            if (id == null)
            {
                y = _dc.Developers.First(x => x.User.Nickname == HttpContext.User.Identity.Name);
                id = y.ID;
            }
            else
            {
                y = _dc.Developers.FirstOrDefault(x => x.ID == id);
            }
            ViewBag.oyun = _dc.Games.Where(x => x.Developer.ID == id).ToList();
            return View(y);
        }

        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            List<Developer> hepsi = _dc.Developers.ToList();
            List<User> bagliKullanici = _dc.Users.ToList();
            foreach (Developer d in hepsi)
            {
                User tmpK = _dc.Users.FirstOrDefault(z => z.ID == d.UserID);
                bagliKullanici.Remove(tmpK);
            }
            ViewBag.kullanici = bagliKullanici;
            Developer y = new Developer();
            return View(y);
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Developer y)
        {
            Developer ypm = _dc.Developers.FirstOrDefault(x => x.ID == y.ID);
            if (ypm == null)
            {
                _dc.Developers.Add(y);
            }
            else
            {
                ypm.Name = y.Name;
                ypm.UserID = y.UserID;
            }
            ypm.User.Role += "Y";
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "A,Y")]
        public ActionResult Guncelle(int id)
        {
            User tmpK;
            List<Developer> hepsi = _dc.Developers.ToList();
            List<User> bagliKullanici = _dc.Users.ToList();
            foreach (Developer d in hepsi)
            {
                tmpK = _dc.Users.FirstOrDefault(z => z.ID == d.ID);
                bagliKullanici.Remove(tmpK);
            }
            tmpK = _dc.Users.FirstOrDefault(x => x.ID == _dc.Developers.FirstOrDefault(z => z.ID == id).UserID);
            bagliKullanici.Remove(tmpK);

            ViewBag.kullanici = bagliKullanici;
            Developer y = _dc.Developers.First(x => x.ID == id);
            ViewBag.yapimci = y;
            return View(y);
        }

        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Guncelle(Developer y)
        {
            var data = _dc.Developers.FirstOrDefault(x => x.ID == y.ID);

            if (data is null)
                return View();

            data.Name = y.Name;
            data.UserID = y.UserID;

            _dc.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            Developer y = _dc.Developers.FirstOrDefault(x => x.ID == id);
            _dc.Developers.Remove(y);
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}