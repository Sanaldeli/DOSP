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
        private readonly Model m = new Model();

        [Authorize]
        public ActionResult Index()
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            List<Kutuphane> l = m.Kutuphanes.Where(x => x.KullaniciID == k.KullaniciID).ToList();
            return View(l);
        }
    }
}