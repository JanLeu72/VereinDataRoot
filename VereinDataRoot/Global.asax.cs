namespace VereinDataRoot
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using Models;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null)
            {
                CultureInfo ci;

                if (Request.UserLanguages != null)
                {
                    try
                    {
                        string ui = new CultureInfo(Request.UserLanguages[0]).Parent.Name;
                        ci = new CultureInfo(ui + "-CH");
                    }
                    catch
                    {
                        ci = new CultureInfo("de-CH");
                    }
                }
                else
                {
                    ci = new CultureInfo("de-CH");
                }

                Thread.CurrentThread.CurrentUICulture = ci;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(ci.Name);
            }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            string action = "";
            string view = "";

            string istController = " ";
            string istAction = " ";

            string errorMess = "";

            HttpContext httpContext = ((MvcApplication)sender).Context;
            RouteData currentRouteData = RouteTable.Routes.GetRouteData(new HttpContextWrapper(httpContext));

            if (currentRouteData != null)
            {
                if (currentRouteData.Values["controller"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["controller"].ToString()))
                {
                    istController = currentRouteData.Values["controller"].ToString();
                }

                if (currentRouteData.Values["action"] != null &&
                    !String.IsNullOrEmpty(currentRouteData.Values["action"].ToString()))
                {
                    istAction = currentRouteData.Values["action"].ToString();
                }
            }

            if (Session["MandantSession"] == null)
            {
                action = "SessionEnd";
                view = "SessionEnd";
                errorMess = "Web-Session ist = null (Abgelaufen)";
            }
            else if (ex.GetType() == typeof(NullReferenceException))
            {
                view = "Error";
                action = "NullReference";
                errorMess = ex.ToString();
            }
            else if (ex.GetType() == typeof(HttpException))
            {
                var httpException = (HttpException)ex;
                view = "Error";
                errorMess = ex.ToString();
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        action = "HttpError404";
                        break;
                    case 500:
                        action = "HttpError500";
                        break;
                    default:
                        action = "HttpErrorXxx";
                        break;
                }
            }
            else
            {
                action = "HttpErrorXxx";
                view = "Error";
                errorMess = ex.ToString();
            }

            Server.ClearError();
            Log.Net.Error("Global: Controller=" + istController + " Action=" + istAction + " Error=" + errorMess);
            Response.Redirect(String.Format("~/{0}/{1}", view, action));
        }
    }
}
