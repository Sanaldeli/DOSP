using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class MagazaController : Controller
    {
        private readonly Model m = new Model();
        public ActionResult Index()
        {
            List<Oyun> o = m.Oyuns.ToList();
            ViewBag.kategori = m.Kategoris.ToList();
            return View(o);
        }
        public ActionResult Ara(string ara)
        {
            ViewBag.ara = ara;
            if (!string.IsNullOrEmpty(ara))
            {
                ViewBag.oyun = m.Oyuns.Where(x => x.OyunAdi.Contains(ara)).ToList();
                ViewBag.yapimci = m.Yapimcis.Where(x => x.YapimciAdi.Contains(ara)).ToList();
            }
            return View();
        }
        public ActionResult Oyun(int id)
        {
            Oyun o = m.Oyuns.FirstOrDefault(x => x.OyunID == id);
            return View(o);
        }
        [Authorize(Roles = "A")]
        public ActionResult Liste()
        {
            List<Oyun> o = m.Oyuns.ToList();
            return View(o);
        }
        [HttpGet]
        [Authorize(Roles = "A,Y")]
        public ActionResult Ekle()
        {
            ViewBag.oyun = m.Oyuns.ToList();
            ViewBag.ktg = m.Kategoris.ToList();
            ViewBag.yapimci = m.Yapimcis.ToList();
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Ekle(Oyun o)
        {
            m.Oyuns.Add(o);
            m.SaveChanges();
            return RedirectToAction("Liste");
        }
        [HttpGet]
        [Authorize(Roles = "A,Y")]
        public ActionResult Duzenle(int id)
        {
            ViewBag.oyun = m.Oyuns.ToList();
            ViewBag.ktg = m.Kategoris.ToList();
            ViewBag.yapimci = m.Yapimcis.ToList();
            using (var context = new DOSPEntities())
            {
                var data = context.Oyuns.Where(x => x.OyunID == id).SingleOrDefault();
                return View(data);
            }
        }
        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Duzenle(Oyun o)
        {
            using (var context = new DOSPEntities())
            {
                var data = context.Oyuns.FirstOrDefault(x => x.OyunID == o.OyunID);

                if (data != null)
                {
                    data.OyunAdi = o.OyunAdi;
                    data.OyunFiyati = o.OyunFiyati;
                    data.OyunAciklama = o.OyunAciklama;
                    data.KategoriID = o.KategoriID;
                    data.YapimciID = o.YapimciID;
                    context.SaveChanges();

                    return RedirectToAction("Liste");
                }
                return View();
            }
        }
        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Sil(int id)
        {
            Oyun o = m.Oyuns.FirstOrDefault(x => x.OyunID == id);
            m.Oyuns.Remove(o);
            m.SaveChanges();
            return RedirectToAction("Liste");
        }
    }
}