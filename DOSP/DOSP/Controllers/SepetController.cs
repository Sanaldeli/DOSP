using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class SepetController : Controller
    {
        private readonly Model m = new Model();

        [Authorize]
        public ActionResult Index()
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            Sepet sepet = m.Sepets.FirstOrDefault(x => x.KullaniciID == k.KullaniciID);
            ViewBag.sid = sepet.SepetID;
            ViewBag.bakiye = k.bakiye;
            List<sepetOyun> sO = m.SepetOyuns.Where(x => x.SepetID == sepet.SepetID && !x.odendi).ToList();
            return View(sO);
        }
        [HttpPost]
        [Authorize]
        public ActionResult Ekle(int id)
        {
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            Sepet s = m.Sepets.FirstOrDefault(x => x.KullaniciID == k.KullaniciID);
            int sepetID = s.SepetID;
            sepetOyun sO = new sepetOyun
            {
                SepetID = sepetID,
                OyunID = id
            };
            m.SepetOyuns.Add(sO);
            m.SaveChanges();
            return RedirectToAction("Index", "Magaza");
        }
        [HttpPost]
        [Authorize]
        public ActionResult Kaldir(int id)
        {
            sepetOyun s = m.SepetOyuns.FirstOrDefault(x => x.ID == id);
            m.SepetOyuns.Remove(s);
            m.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize]
        public ActionResult SatinAl(int id)     //id: SepetID
        {
            List<sepetOyun> s = m.SepetOyuns.Where(x => x.SepetID == id).ToList();
            Kullanici u = m.Kullanicis.FirstOrDefault(x => x.rumuz == HttpContext.User.Identity.Name);
            Kutuphane l = new Kutuphane();
            int total = 0;
            foreach (sepetOyun x in s)
            {
                if (!x.odendi)
                {
                    l.KullaniciID = x.Sepet.KullaniciID;
                    l.OyunID = x.OyunID;
                    m.Kutuphanes.Add(l);
                    m.SaveChanges();
                    total += x.Oyun.OyunFiyati;
                }
                x.odendi = true;
            }
            u.bakiye -= total;
            m.SaveChanges();
            return RedirectToAction("Index", "Kutuphane");
        }
    }
}