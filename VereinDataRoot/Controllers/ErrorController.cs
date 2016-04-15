namespace UFAnet.Controllers
{
    using System.Web.Mvc;

    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult HttpError404()
        {
            @ViewData["Meldung"] = "Die angeforderte Ressource wurde nicht gefunden!";
            @ViewData["NeueAnmeldung"] = "";
            @ViewData["Funktion"] = "Back";

            return View("Error");
        }

        public ActionResult HttpError500()
        {
            @ViewData["Meldung"] = "Es ist ein unerwartete Serverfehler aufgetretten!";
            @ViewData["NeueAnmeldung"] = "Versuchen Sie sich am Portal neu anzumelden.";
            @ViewData["Funktion"] = "Login";

            return View("Error");
        }

        public ActionResult HttpErrorXxx()
        {
            @ViewData["Meldung"] = "Beim Erstellen der Webseite ist ein unerwarteter Fehler aufgetreten!";
            @ViewData["NeueAnmeldung"] = "Versuchen Sie sich am Portal neu anzumelden.";
            @ViewData["Funktion"] = "Login";

            return View("Error");
        }

        public ActionResult NullReference()
        {
            @ViewData["Meldung"] = "Bei versuch Daten abzufragen ist ein Fehler aufgetreten!";
            @ViewData["NeueAnmeldung"] = "Es konnten nicht alle benötigten Parameter für die Abfrage ermittelt werden!";
            @ViewData["Funktion"] = "";

            return View("Error");
        }
    }
}
