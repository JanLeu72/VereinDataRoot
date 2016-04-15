namespace VereinDataRoot.Controllers
{
    using System.Web.Mvc;
    using Models;
    using Repository.Context;
    public class MandantenGruppenController : Controller
    {
        // GET: MandantenGruppen
        public ActionResult MandantenGruppen()
        {
            return PartialView("_MandantenGruppen");
        }

        public JsonResult GetGruppen()
        {
            MandantSession session = (MandantSession) Session["MandantSession"];
            return Json(Repository.Context.MandantenGruppen.GetMandantGruppen(session.MandantId));
        }

        public JsonResult SetGruppe(KeyValueModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            int gruppeId;
            if (!int.TryParse(model.Id, out gruppeId))
            {
                gruppeId = 0;
            }

            MandantGruppe gruppe = new MandantGruppe();
            gruppe.MandantId = session.MandantId;
            gruppe.MandantBenutzerGruppeId = gruppeId;
            gruppe.MandantBenutzerGruppeName = model.Value;

            return Json(Repository.Context.MandantenGruppen.SetMandantGruppe(gruppe));
        }

        public JsonResult DelGruppe(KeyValueModel model)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];

            MandantGruppe gruppe = new MandantGruppe();
            gruppe.MandantId = session.MandantId;
            gruppe.MandantBenutzerGruppeId = int.Parse(model.Id);
            gruppe.MandantBenutzerGruppeName = model.Value;

            Mitglieder mitglieder = new Mitglieder();
            mitglieder.SetMitgliederToDefaultMandantGroupeId(gruppe);

            return Json(Repository.Context.MandantenGruppen.DelMandantGruppe(gruppe));
        }
    }
}