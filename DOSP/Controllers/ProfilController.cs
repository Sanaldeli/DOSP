using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;
using BCryptNet = BCrypt.Net.BCrypt;

namespace DOSP.Controllers
{
    public class ProfilController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index(int? id)
        {
            User k;
            if (id == null)
            {
                k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            }
            else
            {
                k = _dc.Users.FirstOrDefault(x => x.ID == id);
            }
            
            ViewBag.kutuphane = _dc.Libraries.Where(x => x.UserID == k.ID).ToList();
            return View(k);
        }

        [Authorize]
        public ActionResult Duzenle()
        {
            User currentUser = _dc.Users.First(x => x.Nickname == HttpContext.User.Identity.Name);

            if (currentUser is null)
            {
                ViewBag.error = "Kullanıcı bulunamadı.";
                return View();
            }

            return View(currentUser);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Duzenle(User u, string currentPassword, string newPassword, HttpPostedFileBase file)
        {
            User currentUser = _dc.Users.First(x => x.Nickname == HttpContext.User.Identity.Name);

            if (currentUser is null)
            {
                ViewBag.error = "Kullanıcı bulunamadı.";
                return RedirectToAction("Index", "Giris");
            }

            currentUser.FullName = u.FullName;
            currentUser.Wallet = u.Wallet;

            if (!string.IsNullOrEmpty(currentPassword) && !string.IsNullOrEmpty(newPassword))
            {
                if (BCryptNet.Verify(currentPassword, currentUser.Password))
                {
                    currentUser.Password = BCryptNet.HashPassword(newPassword, workFactor: 12);
                }
                else
                {
                    ViewBag.error = "Mevcut parola hatalı girildi.";
                    return View(currentUser);
                }
            }

            if (file != null)
            {
                var randomName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var path = Path.Combine(Server.MapPath("~/Content/Images/users"), randomName);
                currentUser.ProfilePicture = randomName;

                file.SaveAs(path);
            }

            _dc.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}