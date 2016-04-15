using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;

namespace VereinDataRoot.Controllers
{
    public class VorstandController : Controller
    {
        // GET: Vorstand
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Uebersicht()
        {
            return PartialView("_Uebersicht");
        }

        public JsonResult LoadRessorts()
        {
            return Json(Utilitys.GetRessorts());
        }

        public JsonResult LoadVorstand()
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            List<VorstandModel> list = Vorstand.GetVorstaende(session.MandantId);

            return Json(list);
        }

        public JsonResult GetMitglied(string searchText)
        {
            Mitglieder m = new Mitglieder();
            MandantSession session = (MandantSession)Session["MandantSession"];
            List<VorstandModel> list = m.GetSearchMitglieder(searchText, session.MandantId);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SetVorstandMitglied(int ressortId, int mitgliedId)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            return Json(Vorstand.SetVorstandsMitglied(mitgliedId, ressortId, session.MandantId));
        }

        public JsonResult DelVorstandMitglied(VorstandModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            return Json(Vorstand.DelVorstandsMitglied(model.VorstandId, model.MitgliedId, model.RessortId, session.MandantId));
        }
    }
}