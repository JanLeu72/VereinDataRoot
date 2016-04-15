using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Repository.Context;

namespace VereinDataRoot.Controllers
{
    public class BenutzerVerwaltungController : Controller
    {
        // GET: BenutzerVerwalltung
        public ActionResult BenutzerUebersicht()
        {
            return PartialView("_BenutzerUebersicht");
        }

        public JsonResult LoadBenutzers()
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            return Json(Benutzer.GetBenutzers(session.MandantId));
        }

        public JsonResult SetBenutzer(BenutzerModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            model.Passwort = "billabong";
            model.MandantId = session.MandantId;

            return Json(Benutzer.SetBenutzer(model));
        }

        public JsonResult DelBenutzer(BenutzerModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            model.MandantId = session.MandantId;
            return Json(Benutzer.DelBenutzer(model));
        }

        public JsonResult LoadBenutzerModule(GridFilters filter = null)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            int benutzerId = 0;
            if (filter != null
                && filter.Filters != null)
            {
                benutzerId = int.Parse(filter.Filters[0].Value);
            }

            return Json(Benutzer.GetBenutzerMandantModule(benutzerId, session.MandantId));
        }

        public JsonResult SetBenutzerModul(int benutzerId, int modulId, bool check)
        {
            return Json(Benutzer.SetBenutzerModul(benutzerId, modulId, check));
        }
    }
}