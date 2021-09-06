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
        private readonly Model m = new Model();

        [Authorize]
        public ActionResult Index(int? id)
        {
            Kullanici k;
            if (id == null)
            {
                k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            }
            else
            {
                k = m.Kullanicis.FirstOrDefault(x => x.KullaniciID == id);
            }
            
            ViewBag.kutuphane = m.Kutuphanes.Where(x => x.KullaniciID == k.KullaniciID).ToList();
            return View(k);
        }
    }
}