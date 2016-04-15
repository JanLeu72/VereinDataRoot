namespace VereinDataRoot.Controllers
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Repository.Context;
    using Models;
    using Helpers;
    using ViewModels;
    public class MainController : Controller
    {
        //
        // GET: /Main/
        //public ActionResult TreeNavigation()
        //{
        //    //List<TreeNavigationModel> list = Hierarchie.FillRecursive(Menue.GetMainNavigation(), 0);
        //    //return PartialView("_MainNavigation", list);

        //    return PartialView("_MainNavigation");
        //}

        public ActionResult TreeNav(int id)
        {
            List<TreeNavigationModel> list = Hierarchie.FillRecursive(Menue.GetMainNavigation(), id);
            return PartialView("_TreeNav", list);
        }

        public ActionResult Header()
        {
            MandantSession session = (MandantSession) Session["MandantSession"];
            HeaderViewModel model = new HeaderViewModel();

           model.Navigation = Menue.GetMandantenBenutzerNavigation(session.BenutzerId, session.MandantId);

            model.VereinName = session.MandantName;
            model.UserName = session.BenutzerName;
            return PartialView("_Header", model);
        }

        public JsonResult GetAnreden()
        {
            return Json(Utilitys.GetAnreden(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMitgliedschaften()
        {
            return Json(Utilitys.GetMitgliedschaftTypen(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMandantenGruppen()
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            return Json(MandantenGruppen.GetMandantGruppenForAll(session.MandantId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLaender()
        {
            return Json(Utilitys.GetLaender(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRessorts()
        {
            return Json(Utilitys.GetRessorts(), JsonRequestBehavior.AllowGet);
        }
	}
}