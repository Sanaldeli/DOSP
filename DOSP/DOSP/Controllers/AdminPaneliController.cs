using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOSP.Models;

namespace DOSP.Controllers
{
    public class AdminPaneliController : Controller
    {
        [Authorize(Roles = "A")]
        public ActionResult Index()
        {
            return View();
        }
    }
}