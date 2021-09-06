using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class KategoriController : Controller
    {
        private readonly Model m = new Model();

        [Authorize]
        public ActionResult Index()
        {
            List<Kategori> k = m.Kategoris.ToList();
            return View(k);
        }
        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            Kategori k = new Kategori();
            return View(k);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Kategori k)
        {
            Kategori ktg = m.Kategoris.FirstOrDefault(x => x.KategoriID == k.KategoriID);
            if (ktg == null)
            {
                m.Kategoris.Add(k);
            }
            else
            {
                ktg.KategoriAdi = k.KategoriAdi;
            }
            m.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            Kategori k = m.Kategoris.FirstOrDefault(x => x.KategoriID == id);
            return View("Ekle", k);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            Kategori k = m.Kategoris.FirstOrDefault(x => x.KategoriID == id);
            m.Kategoris.Remove(k);
            m.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}