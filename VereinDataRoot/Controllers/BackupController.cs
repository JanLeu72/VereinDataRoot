using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace VereinDataRoot.Controllers
{
    public class BackupController : Controller
    {
        // GET: Backup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Erstellen()
        {
            MandantSession session = (MandantSession) Session["MandantSession"];
            return PartialView("_Erstellen", session);
        }
    }
}