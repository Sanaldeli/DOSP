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
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Gecmis()
        {
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            ViewBag.Ticket = _dc.Tickets.Where(x => x.UserID == k.ID).ToList();
            ViewBag.Rapor = _dc.Reports.Where(x => x.UserID == k.ID).ToList();
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
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);

            if (!ModelState.IsValid)
                return View();

            t.UserID = k.ID;
            t.CreatedAt = DateTime.Now;
            _dc.Configuration.ValidateOnSaveEnabled = false;
            _dc.Tickets.Add(t);
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult Rapor(int id)
        {
            ViewBag.oyun = _dc.Games.FirstOrDefault(x => x.ID == id);
            List<ReportCategory> rK = _dc.ReportCategories.ToList();
            return View(rK);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Rapor(Report r)
        {
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);

            if (!ModelState.IsValid)
                return View();

            r.UserID = k.ID;
            r.ReportDate = DateTime.Now;
            _dc.Configuration.ValidateOnSaveEnabled = false;
            _dc.Reports.Add(r);
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult RaporGoruntule(int id)
        {
            Report r = _dc.Reports.FirstOrDefault(x => x.ID == id);
            return View(r);
        }

        public ActionResult TicketGoruntule(int id)
        {
            Ticket t = _dc.Tickets.FirstOrDefault(x => x.ID == id);
            return View(t);
        }
    }
}