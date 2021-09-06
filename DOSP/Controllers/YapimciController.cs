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
        private readonly Model m = new Model();

        [Authorize(Roles = "A,Y")]
        public ActionResult Index()
        {
            List<Yapimci> y = m.Yapimcis.ToList();
            return View(y);
        }
        public ActionResult Firma(int? id)
        {
            Yapimci y;
            if (id == null)
            {
                y = m.Yapimcis.FirstOrDefault(x => x.Kullanici.rumuz == HttpContext.User.Identity.Name);
                id = y.YapimciID;
            }
            else
            {
                y = m.Yapimcis.FirstOrDefault(x => x.YapimciID == id);
            }
            ViewBag.oyun = m.Oyuns.Where(x => x.Yapimci.YapimciID == id).ToList();
            return View(y);
        }
        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            List<Yapimci> hepsi = m.Yapimcis.ToList();
            List<Kullanici> bagliKullanici = m.Kullanicis.ToList();
            foreach (Yapimci x in hepsi)
            {
                Kullanici tmpK = m.Kullanicis.FirstOrDefault(z => z.KullaniciID == x.KullaniciID);
                bagliKullanici.Remove(tmpK);
            }
            ViewBag.kullanici = bagliKullanici;
            Yapimci y = new Yapimci();
            return View(y);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Yapimci y)
        {
            Yapimci ypm = m.Yapimcis.FirstOrDefault(x => x.YapimciID == y.YapimciID);
            if (ypm == null)
            {
                m.Yapimcis.Add(y);
            }
            else
            {
                ypm.YapimciAdi = y.YapimciAdi;
                ypm.KullaniciID = y.KullaniciID;
            }
            m.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Roles = "A,Y")]
        public ActionResult Guncelle(int id)
        {
            Kullanici tmpK;
            List<Yapimci> hepsi = m.Yapimcis.ToList();
            List<Kullanici> bagliKullanici = m.Kullanicis.ToList();
            foreach (Yapimci x in hepsi)
            {
                tmpK = m.Kullanicis.FirstOrDefault(z => z.KullaniciID == x.KullaniciID);
                bagliKullanici.Remove(tmpK);
            }
            tmpK = m.Kullanicis.FirstOrDefault(x => x.KullaniciID == m.Yapimcis.FirstOrDefault(z => z.YapimciID == id).KullaniciID);
            bagliKullanici.Remove(tmpK);

            ViewBag.kullanici = bagliKullanici;
            Yapimci y = m.Yapimcis.FirstOrDefault(x => x.YapimciID == id);
            return View(y);
        }
        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Guncelle(Yapimci y)
        {
            using (var context = new DOSPEntities())
            {
                var data = context.Yapimcis.FirstOrDefault(x => x.YapimciID == y.YapimciID);

                if (data != null)
                {
                    data.YapimciAdi = y.YapimciAdi;
                    data.KullaniciID = y.KullaniciID;

                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            Yapimci y = m.Yapimcis.FirstOrDefault(x => x.YapimciID == id);
            m.Yapimcis.Remove(y);
            m.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}