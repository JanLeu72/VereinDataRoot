using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VereinDataRoot.Controllers
{
    public class EinstellungVereinController : Controller
    {
        // GET: EinstellungVerein
        public ActionResult Einstellung()
        {
            return PartialView("_Einstellung");
        }
    }
}