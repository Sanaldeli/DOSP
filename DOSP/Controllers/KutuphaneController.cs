using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class KutuphaneController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index()
        {
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            List<Library> l = _dc.Libraries.Where(x => x.UserID == k.ID).ToList();
            return View(l);
        }
    }
}