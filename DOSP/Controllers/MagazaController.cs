using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DOSP.Models;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Web;

namespace DOSP.Controllers
{
    public class MagazaController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        public ActionResult Index()
        {
            List<Game> o = _dc.Games.ToList();
            ViewBag.kategori = _dc.Categories.ToList();
            return View(o);
        }

        public ActionResult Ara(string ara)
        {
            ViewBag.ara = ara;
            if (!string.IsNullOrEmpty(ara))
            {
                ViewBag.oyun = _dc.Games.Where(x => x.Title.Contains(ara)).ToList();
                ViewBag.yapimci = _dc.Developers.Where(x => x.Name.Contains(ara)).ToList();
            }

            return View();
        }

        public ActionResult Oyun(int id)
        {
            Game o = _dc.Games.FirstOrDefault(x => x.ID == id);
            return View(o);
        }

        [Authorize(Roles = "A")]
        public ActionResult Liste()
        {
            List<Game> o = _dc.Games.ToList();
            return View(o);
        }

        [HttpGet]
        [Authorize(Roles = "Y")]
        public ActionResult Ekle()
        {
            ViewBag.ktg = _dc.Categories.ToList();
            Developer d = _dc.Developers.First(x => x.User.Nickname == HttpContext.User.Identity.Name);

            return View(d);
        }

        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Ekle(Game o, HttpPostedFileBase file)
        {
            if (file != null)
            {
                var randomName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(Server.MapPath("~/Content/Images/gcont"), randomName);
                o.CoverPhoto = randomName;

                file.SaveAs(path);
            }
            else
            {
                o.CoverPhoto = "game-photo.jpg";
            }

            _dc.Games.Add(o);

            _dc.SaveChanges();

            return RedirectToAction("Liste");
        }

        [HttpGet]
        [Authorize(Roles = "A,Y")]
        public ActionResult Duzenle(int id)
        {
            ViewBag.oyun = _dc.Games.ToList();
            ViewBag.ktg = _dc.Categories.ToList();

            var data = _dc.Games.Where(x => x.ID == id).SingleOrDefault();
            ViewBag.yapimci = _dc.Developers.FirstOrDefault(d => d.User.Nickname == HttpContext.User.Identity.Name);

            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Duzenle(Game o, HttpPostedFileBase file)
        {
            var data = _dc.Games.FirstOrDefault(x => x.ID == o.ID);

            if (data is null)
            {
                ViewBag.error = "Oyun bulunamadı.";
                return View();
            }

            data.Title = o.Title;
            data.Price = o.Price;
            data.Description = o.Description;
            data.CategoryID = o.CategoryID;
            data.ReleaseDate = o.ReleaseDate;

            if (file != null)
            {
                var randomName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(Server.MapPath("~/Content/Images/gcont"), randomName);
                data.CoverPhoto = randomName;

                file.SaveAs(path);
            }

            _dc.SaveChanges();

            return RedirectToAction("Liste");
        }

        [HttpPost]
        [Authorize(Roles = "A,Y")]
        public ActionResult Sil(int id)
        {
            Game o = _dc.Games.FirstOrDefault(x => x.ID == id);
            _dc.Games.Remove(o);
            _dc.SaveChanges();
            return RedirectToAction("Liste");
        }
    }
}