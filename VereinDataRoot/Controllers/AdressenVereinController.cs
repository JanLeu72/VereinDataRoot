using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Repository.Context;

namespace VereinDataRoot.Controllers
{
    public class AdressenVereinController : Controller
    {
        // GET: AdressenVerein
        public ActionResult Verein()
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            VereinAdresse model = VereinsAdressen.GetVereinAdresse(session.MandantId, 1);

            if (model == null)
            {
                model = new VereinAdresse();
                model.Postfach = "";
                model.Name = "";
                model.Vorname = "";
                model.Strasse = "";
                model.Plz = "";
                model.Ort = "";
                model.VereinAdresseId = 0;
            }
            model.MandantName = session.MandantName.Trim();
            model.FormMessage = "";

            return PartialView("_Verein", model);
        }
        public ActionResult Kontaktperson()
        {
            VereinAdresse model = new VereinAdresse();
            model.Anreden = Utilitys.GetAnreden();
            return PartialView("_Kontaktperson");
        }
        public ActionResult RechnungsAdresse()
        {
            return PartialView("_RechnungsAdresse");
        }

        [HttpPost]
        public JsonResult SetVereinAdresse(VereinAdresse model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            if (VereinsAdressen.SetAdresse(session.MandantId, 1, model))
            {
                model.FormMessage = "Adresse erfolgreich gespeicht!";
            }
            else
            {
                model.FormMessage = "Adresse konnte nicht gespeicht werden!";
            }

            return Json(model);
        }
    }
}