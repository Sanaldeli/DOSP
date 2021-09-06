using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly Model m = new Model();

        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            List<Kullanici> k = m.Kullanicis.ToList();
            return View(k);
        }
        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            using (var context = new DOSPEntities())
            {
                var data = context.Kullanicis.Where(x => x.KullaniciID == id).SingleOrDefault();
                return View(data);
            }
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(Kullanici k, bool admin, bool yapimci)
        {
            using (var context = new DOSPEntities())
            {
                var data = context.Kullanicis.FirstOrDefault(x => x.KullaniciID == k.KullaniciID);

                if (data != null)
                {
                    data.rumuz = k.rumuz;
                    data.adSoyad = k.adSoyad;
                    data.bakiye = k.bakiye;
                    data.sifre = k.sifre;

                    string roller = "K";
                    if (admin)
                    {
                        roller += "A";
                    }
                    if (yapimci)
                    {
                        roller += "Y";
                    }
                    data.Rol = roller;

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
            Kullanici k = m.Kullanicis.FirstOrDefault(x => x.KullaniciID == id);
            m.Kullanicis.Remove(k);
            m.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}