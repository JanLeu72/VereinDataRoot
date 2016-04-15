using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Models;
using Repository.Context;
using VereinDataRoot.ViewModels;

namespace VereinDataRoot.Controllers
{
    public class AdminOptionsController : Controller
    {
        // GET: AdminOptions
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PasswortAendern()
        {
            PasswortViewModel model = new PasswortViewModel();
            model.Passwort1 = "";
            model.Passwort2 = "";
            model.MessPass = "";

            return PartialView("_PasswortAendern", model);
        }

        public JsonResult SetPw(PasswortViewModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            if (Benutzer.SetBenutzerPasswort(session.BenutzerId, session.MandantId, model.Passwort1))
            {
                return Json("Passwort erfolgreich geändert.");
            }
            
            return Json("Passwort konnte nicht geändert werden!");
        }
    }
}