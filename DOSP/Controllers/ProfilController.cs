using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class ProfilController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index(int? id)
        {
            User k;
            if (id == null)
            {
                k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            }
            else
            {
                k = _dc.Users.FirstOrDefault(x => x.ID == id);
            }
            
            ViewBag.kutuphane = _dc.Libraries.Where(x => x.UserID == k.ID).ToList();
            return View(k);
        }
    }
}