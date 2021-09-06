using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class DestekController : Controller
    {
        private readonly Model m = new Model();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Gecmis()
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            ViewBag.Ticket = m.Tickets.Where(x => x.KullaniciID == k.KullaniciID).ToList();
            ViewBag.Rapor = m.Rapors.Where(x => x.KullaniciID == k.KullaniciID).ToList();
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Ticket()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Ticket(Ticket t)
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);

            if (ModelState.IsValid)
            {
                using (DOSPEntities db = new DOSPEntities())
                {
                    t.KullaniciID = k.KullaniciID;
                    t.TicketTarihi = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Tickets.Add(t);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult Rapor(int id)
        {
            ViewBag.oyun = m.Oyuns.FirstOrDefault(x => x.OyunID == id);
            List<RaporKategori> rK = m.RaporKategoris.ToList();
            return View(rK);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Rapor(Rapor r)
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);

            if (ModelState.IsValid)
            {
                using (DOSPEntities db = new DOSPEntities())
                {
                    r.KullaniciID = k.KullaniciID;
                    r.RaporTarihi = DateTime.Now;
                    db.Configuration.ValidateOnSaveEnabled = false;
                    db.Rapors.Add(r);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [Authorize]
        public ActionResult RaporGoruntule(int id)
        {
            Rapor r = m.Rapors.FirstOrDefault(x => x.RaporID == id);
            return View(r);
        }
        public ActionResult TicketGoruntule(int id)
        {
            Ticket t = m.Tickets.FirstOrDefault(x => x.TicketID == id);
            return View(t);
        }
    }
}