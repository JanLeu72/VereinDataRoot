namespace VereinDataRoot.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using CsvHelper;
    using CsvHelper.Configuration;
    using Models;
    using Repository.Context;
    using ViewModels;

    public class MitgliederController : Controller
    {
        //
        // GET: /Mitglieder/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Uebersicht()
        {
            return PartialView("_Uebersicht");
        }

        public ActionResult NeuesMitglied()
        {
            MandantSession session = (MandantSession) Session["MandantSession"];
            NeuesMitgliedViewModel model = new NeuesMitgliedViewModel();
            model.Anreden = Utilitys.GetAnreden();
            model.MitgliedschaftTypen = Utilitys.GetMitgliedschaftTypen();
            model.MandantenGruppen = MandantenGruppen.GetMandantGruppenForAll(session.MandantId);
            model.Laender = Utilitys.GetLaender();
            model.LandId = 0;
            model.MitgliedId = 0;

            return PartialView("_NeuesMitglied", model);
        }

        public JsonResult DelMitglied(MitgliedModel model)
        {
            if (model.MitgliedId > 0 &&
                Session["MandantSession"] != null)
            {
                model.MandantId = ((MandantSession) Session["MandantSession"]).MandantId;

                Mitglieder mitglieder = new Mitglieder();
                return  Json(mitglieder.DelMitgliedById(model));
            }

            return Json(false);
        }

        [HttpPost]
        public JsonResult SetMitglied(MitgliedModel model)
        {
            if (Session["MandantSession"] != null)
            {
                MandantSession session = (MandantSession)Session["MandantSession"];
                model.MandantId = session.MandantId;

                if (string.IsNullOrWhiteSpace(model.Vorname))
                {
                    model.Vorname = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    model.Name = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.ZusatzName))
                {
                    model.ZusatzName = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Strasse))
                {
                    model.Strasse = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Plz))
                {
                    model.Plz = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Ort))
                {
                    model.Ort = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Mail))
                {
                    model.Mail = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.TelefonP))
                {
                    model.TelefonP = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.TelefonG))
                {
                    model.TelefonG = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Mobile))
                {
                    model.Mobile = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Fax))
                {
                    model.Fax = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Geburtstag))
                {
                    model.Geburtstag = string.Empty;
                }
                if (string.IsNullOrWhiteSpace(model.Bemerkung))
                {
                    model.Bemerkung = string.Empty;
                }
                DateTime rd;
                if (DateTime.TryParse(model.RechnungsDatumString, out rd))
                {
                    model.RechnungsDatum = rd.Date;
                }
                else
                {
                    model.RechnungsDatum = DateTime.Now.Date;
                }

                bool save = false;
                int maxMandantMitglieder = 0;
                int istMitglieder = 0;
                int mitgliederDiverenz = 0;
                string mess = "";
                Mitglieder mitglieder = new Mitglieder();

                if (model.MitgliedId > 0)
                {
                    save = mitglieder.SetMitglied(model);

                    return Json(new
                    {
                        save = save,
                        maxMa = maxMandantMitglieder,
                        isMit = istMitglieder,
                        divMit = mitgliederDiverenz,
                        mess = "Mitglied gespeichert"
                    });
                }
                else
                {
                    if (mitglieder.GetMaxMitglieder(session.MandantId,
                        out maxMandantMitglieder,
                        out istMitglieder,
                        out mitgliederDiverenz))
                    {
                        save = mitglieder.SetMitglied(model);

                        return Json(new
                        {
                            save = save,
                            maxMa = maxMandantMitglieder,
                            isMit = istMitglieder,
                            divMit = mitgliederDiverenz,
                            mess = "Mitglied gespeichert"
                        });
                    }
                    else
                    {
                        return Json(new
                        {
                            save = false,
                            maxMa = maxMandantMitglieder,
                            isMit = istMitglieder,
                            divMit = mitgliederDiverenz,
                            mess = "Beim versuch das Mitglied zu speichern ist ein Fehler aufgetreten!"
                        });
                    }
                }
            }

            return Json(new
            {
                save = false,
                maxMa = 0,
                isMit = 0,
                divMit = 0,
                mess = "Beim versuch das Mitglied zu speichern ist ein Fehler aufgetreten!"
            });
        }

        public ActionResult MaxMitglieder(string maxMa, string isMit, string divMit, string mess)
        {
            MaxMitgliederViewModel model = new MaxMitgliederViewModel();
            model.MaxMitglieder = maxMa;
            model.IstMitglieder = isMit;
            model.DivMitglieder = divMit;
            model.Message = mess;

            return PartialView("_MaxMitglieder", model);
        }

        public JsonResult SetNewAbo(int value)
        {
            MandantSession session = (MandantSession)Session["MandantSession"];
            Mitglieder mitglieder = new Mitglieder();

            if (value > 0)
            {
                return Json(mitglieder.SetNewMandantenAbo(session.MandantId, value));
            }

            return Json(false);
        }

        public JsonResult GetMitglieder(int skip, int take, GridFilters filter = null, List<TableSort> sort = null)
        {
            MandantSession session = (MandantSession) Session["MandantSession"];

            MitgliedRequest request = new MitgliedRequest();
            request.MandantId = session.MandantId;
            request.Skip = skip;
            request.Take = take;
            request.Filters = new List<TableFilter>();
            request.Sorting = new List<TableSort>();

            if (sort != null && sort.Count > 0)
            {
                request.Sorting = sort;
            }

            if (filter != null
                && filter.Filters != null)
            {
                request.Filters = filter.Filters;
            }

            Mitglieder mitglieder = new Mitglieder();
            MitgliedGrid l = mitglieder.GetMitglieder(request);

            return Json(l, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetNewDate(string date, bool b)
        {
            DateTime newDate;
            if (DateTime.TryParse(date, out newDate))
            {
                if (b)
                {
                    newDate = newDate.AddYears(1);
                }
                else
                {
                    newDate = newDate.AddYears(-1);
                }
            }
            else
            {
                newDate = DateTime.Now;
            }

            return Json(newDate.ToShortDateString());
        }

        public void GetCsv(string guid)
        {
            HttpContext context = System.Web.HttpContext.Current;

            try
            {
                string fileName = "Mitglieder.csv";

                byte[] bytes = (byte[]) Session[guid];

                System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition",
                    "attachment;filename=\"" + fileName + "\"");
                context.Response.AddHeader("Content-Length", bytes.Length.ToString());
                context.Response.ContentType = "text/comma-separated-values";
                context.Response.BinaryWrite(bytes);
                context.Response.Flush();
                context.Response.Close();
            }
            catch (Exception ex)
            {
                Log.Net.Error("GetCsv Mitglieder: " + ex);
                context.Response.ContentType = "text/plain";
                context.Response.Write("{\"name\":\"Fehler Datei!\"}");
            }
        }

        public JsonResult ExportMitglieder(GridFilters filter, string guid)
        {
            try
            {
                MandantSession session = (MandantSession) Session["MandantSession"];

                MitgliedRequest request = new MitgliedRequest();
                request.MandantId = session.MandantId;
                request.Skip = 0;
                request.Take = int.MaxValue;
                request.Filters = new List<TableFilter>();
                request.Sorting = new List<TableSort>();

                if (filter != null
                    && filter.Filters != null)
                {
                    request.Filters = filter.Filters;
                }

                Mitglieder mitglieder = new Mitglieder();
                MitgliedGrid l = mitglieder.GetMitglieder(request);

                using (MemoryStream memoryStream = new MemoryStream())
                using (StreamWriter streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
                using (CsvWriter csv = new CsvWriter(streamWriter))
                {
                    csv.Configuration.Delimiter = ";";
                    csv.Configuration.CountBytes = false;
                    csv.Configuration.Encoding = Encoding.UTF8;
                    csv.Configuration.HasHeaderRecord = true;
                    csv.Configuration.RegisterClassMap<MitgliedModelMap>();

                    csv.WriteHeader<MitgliedModel>();
                    csv.WriteRecords(l.Mitglieder);
                    csv.Dispose();

                    byte[] bytes = memoryStream.ToArray();
                    Session[guid] = bytes;
                    return Json(true);
                }
            }
            catch (Exception ex)
            {
                Log.Net.Error("ExportMitglieder für CSV-Mitglieder: " + ex);
                return Json(false);
            }
        }

        private class MitgliedModelMap : CsvClassMap<MitgliedModel>
        {
            public override void CreateMap()
            {
                Map(m => m.Anrede).Name("Anrede");
                Map(m => m.Name).Name("Name");
                Map(m => m.Vorname).Name("Vorname");
                Map(m => m.ZusatzName).Name("Zusatz Name");
                Map(m => m.Strasse).Name("Strasse");
                Map(m => m.Plz).Name("Plz");
                Map(m => m.Ort).Name("Ort");
                Map(m => m.Geburtstag).Name("Geburtstag");
                Map(m => m.Mail).Name("Mail");
                Map(m => m.TelefonP).Name("Telefon P");
                Map(m => m.TelefonP).Name("Telefon G");
                Map(m => m.Mobile).Name("Mobile");
                Map(m => m.Fax).Name("Fax");
                Map(m => m.MitgliedschaftTypeName).Name("Mitgliedschaft");
                Map(m => m.MandantBenutzerGruppeName).Name("Gruppe");
                Map(m => m.RechnungsDatumString).Name("Rechnung");
                Map(m => m.LandName).Name("Land");
                Map(m => m.Bemerkung).Name("Bemerkung");
            }
        }
    }
}