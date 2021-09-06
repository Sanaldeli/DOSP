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
        private readonly Model m = new Model();

        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "A")]
        public ActionResult ARapor()
        {
            List<Rapor> r = m.Rapors.ToList();
            return View(r);
        }
        [Authorize(Roles = "A")]
        public ActionResult ARaporGoruntule(int id)
        {
            Rapor r = m.Rapors.FirstOrDefault(x => x.RaporID == id);
            return View(r);
        }
        [Authorize(Roles = "A")]
        public ActionResult ATicket()
        {
            List<Ticket> t = m.Tickets.ToList();
            return View(t);
        }
        [Authorize(Roles = "A")]
        public ActionResult ATicketGoruntule(int id)
        {
            Ticket t = m.Tickets.FirstOrDefault(x => x.TicketID == id);
            return View(t);
        }
    }
}