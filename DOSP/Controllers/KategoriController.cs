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
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index()
        {
            List<Category> k = _dc.Categories.ToList();
            return View(k);
        }

        [HttpGet]
        [Authorize(Roles = "A")]
        public ActionResult Ekle()
        {
            Category k = new Category();
            return View(k);
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Ekle(Category k)
        {
            Category ktg = _dc.Categories.FirstOrDefault(x => x.ID == k.ID);
            if (ktg == null)
            {
                _dc.Categories.Add(k);
            }
            else
            {
                ktg.Name = k.Name;
            }
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "A")]
        public ActionResult Guncelle(int id)
        {
            Category k = _dc.Categories.FirstOrDefault(x => x.ID == id);
            return View("Ekle", k);
        }

        [HttpPost]
        [Authorize(Roles = "A")]
        public ActionResult Sil(int id)
        {
            Category k = _dc.Categories.FirstOrDefault(x => x.ID == id);
            _dc.Categories.Remove(k);
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}