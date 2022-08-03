using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class AdminDestekController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "A")]
        public ActionResult ARapor()
        {
            List<Report> r = _dc.Reports.ToList();
            return View(r);
        }

        [Authorize(Roles = "A")]
        public ActionResult ARaporGoruntule(int id)
        {
            Report r = _dc.Reports.FirstOrDefault(x => x.ID == id);
            return View(r);
        }

        [Authorize(Roles = "A")]
        public ActionResult ATicket()
        {
            List<Ticket> t = _dc.Tickets.ToList();
            return View(t);
        }

        [Authorize(Roles = "A")]
        public ActionResult ATicketGoruntule(int id)
        {
            Ticket t = _dc.Tickets.FirstOrDefault(x => x.ID == id);
            return View(t);
        }
    }
}