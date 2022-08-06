using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class SepetController : Controller
    {
        private readonly DataContext _dc = new DataContext();

        [Authorize]
        public ActionResult Index()
        {
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            Basket sepet = _dc.Baskets.FirstOrDefault(x => x.UserID == k.ID);
            ViewBag.sid = sepet.BasketID;
            ViewBag.bakiye = k.Wallet;
            List<BasketGame> sO = _dc.BasketGames.Where(x => x.BasketID == sepet.BasketID && !x.isPaid).ToList();
            return View(sO);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Ekle(int id)
        {
            User k = _dc.Users.FirstOrDefault(x => x.Nickname == HttpContext.User.Identity.Name);
            Basket s = _dc.Baskets.FirstOrDefault(x => x.UserID == k.ID);
            int sepetID = s.BasketID;
            BasketGame sO = new BasketGame
            {
                BasketID = sepetID,
                GameID = id
            };
            _dc.BasketGames.Add(sO);
            _dc.SaveChanges();
            return RedirectToAction("Index", "Magaza");
        }

        [HttpPost]
        [Authorize]
        public ActionResult Kaldir(int id)
        {
            BasketGame s = _dc.BasketGames.FirstOrDefault(x => x.ID == id);
            _dc.BasketGames.Remove(s);
            _dc.SaveChanges();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Authorize]
        public ActionResult SatinAl(int id)     //id: SepetID
        {
            List<BasketGame> s = _dc.BasketGames.Where(x => x.BasketID == id).ToList();
            User u = _dc.Users.First(x => x.Nickname == HttpContext.User.Identity.Name);
            int total = 0;
            foreach (var bG in s)
            {
                if (!bG.isPaid)
                {
                    _dc.Libraries.Add(new Library
                    {
                        UserID = bG.Basket.UserID,
                        GameID = bG.GameID
                    });
                    total += bG.Game.Price;
                }
                bG.isPaid = true;
            }
            u.Wallet -= total;
            _dc.SaveChanges();
            return RedirectToAction("Index", "Kutuphane");
        }
    }
}