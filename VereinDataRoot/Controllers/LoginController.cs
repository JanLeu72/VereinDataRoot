namespace VereinDataRoot.Controllers
{
    using System.Web.Mvc;
    using Models;
    using Repository.Context;
    using ViewModels;
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            LoginViewModel model = new LoginViewModel();
            model.Us = "";
            model.Pa = "";
            model.Mes = "";
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(LoginViewModel model)
        {
            int error = 0;

            if (string.IsNullOrWhiteSpace(model.Us))
            {
                error += 1;
            }
            if (string.IsNullOrWhiteSpace(model.Pa))
            {
                error += 1;
            }

            if (error == 0)
            {
                BenutzerModel m = new BenutzerModel();
                m.BenutzerMail = model.Us;
                m.Passwort = model.Pa;

                Benutzer benutzer = new Benutzer();
                MandantSession login = benutzer.GetBenutzerLogin(m);
                if (login != null)
                {
                    Session["MandantSession"] = login;
                    return RedirectToAction("Index", "Mitglieder");
                }
            }

            model.Mes = "Login Fehlgeschlagen!";
            model.Pa = "";

            return View(model);
        }

        public ActionResult Demo()
        {
            BenutzerModel m = new BenutzerModel();
            m.BenutzerMail = "verein@dataroot.ch";
            m.Passwort = "billabong";

            Benutzer benutzer = new Benutzer();
            MandantSession login = benutzer.GetBenutzerLogin(m);
            if (login != null)
            {
                Session["MandantSession"] = login;
                return RedirectToAction("Index", "Mitglieder");
            }

            LoginViewModel model = new LoginViewModel();
            model.Us = "";
            model.Pa = "";
            model.Mes = "";
            return View("Index", model);
        }

        public ActionResult Cross(int id)
        {
            if (id > 0)
            {
                Benutzer benutzer = new Benutzer();
                MandantSession login = benutzer.GetCrossLogin(id);
                if (login != null)
                {
                    Session["MandantSession"] = login;
                    return RedirectToAction("Index", "Mitglieder");
                }
            }

            LoginViewModel model = new LoginViewModel();
            model.Us = "";
            model.Pa = "";
            model.Mes = "";
            return View("Index", model);
        }
    }
}