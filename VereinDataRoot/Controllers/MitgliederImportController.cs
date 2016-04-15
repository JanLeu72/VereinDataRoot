using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using Models;
using Repository.Context;
using VereinDataRoot.ViewModels;

namespace VereinDataRoot.Controllers
{
    public class MitgliederImportController : Controller
    {
        // GET: ImportTest
        public JsonResult CsvUpload()
        {
            if (Session["MandantSession"] != null)
            {
                MandantSession session = (MandantSession) Session["MandantSession"];

                if (HttpContext.Request.Files.Count != 0)
                {
                    HttpPostedFileBase filedata = HttpContext.Request.Files["csvfile"];
                    string updPath = ConfigurationSettings.AppSettings["UploadMitgliederCsv"];
                    updPath = updPath + Guid.NewGuid() + ".csv";
                    filedata.SaveAs(updPath);
                    Session["CsvFile"] = updPath;

                    int countFields = 0;

                    using (ICsvParser csvParser = new CsvParser(new StreamReader(updPath, Encoding.UTF7)))
                    {
                        using (CsvReader csv = new CsvReader(csvParser))
                        {
                            csv.Configuration.Encoding = Encoding.UTF8;
                            csv.Configuration.Delimiter = ";";
                            csv.Configuration.SkipEmptyRecords = true;
                            csv.Read();

                            while (csv.Read())
                            {
                                countFields += 1;
                            }
                        }
                    }

                    Mitglieder mitglieder = new Mitglieder();

                    bool save = false;
                    int maxMandantMitglieder = 0;
                    int istMitglieder = 0;
                    int mitgliederDiverenz = 0;
                    string mess = "";

                    mitglieder.GetMaxMitglieder(session.MandantId,
                        out maxMandantMitglieder,
                        out istMitglieder,
                        out mitgliederDiverenz);

                    istMitglieder = countFields;

                    if (istMitglieder < maxMandantMitglieder)
                    {
                        return Json(new
                        {
                            save = true,
                            maxMa = maxMandantMitglieder,
                            isMit = istMitglieder,
                            divMit = mitgliederDiverenz,
                            mess = ""
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
                            mess = ""
                        });
                    }
                }
            }

            return Json(true);
        }

        public ActionResult MaxImport(string maxMa, string isMit, string divMit, string mess)
        {
            MaxMitgliederViewModel model = new MaxMitgliederViewModel();
            model.MaxMitglieder = maxMa;
            model.IstMitglieder = isMit;
            model.DivMitglieder = divMit;
            model.Message = mess;

            return PartialView("_MaxImport", model);
        }

        public JsonResult CsvFileSave(FormCollection form)
        {
            List<MitgliedImportErrorViewModel> importModel = new List<MitgliedImportErrorViewModel>();
            MitgliedModel errorItem;
            string errorMessage;
            int totalMitglieder = 0;
            int totalError = 0;

            if (Session["MandantSession"] != null
                && Session["CsvFile"] != null)
            {
                string updPath = Session["CsvFile"].ToString();
                MandantSession session = (MandantSession) Session["MandantSession"];
                string anrede = string.Empty;
                string vorname = string.Empty;
                string name = string.Empty;
                string namezusatz = string.Empty;
                string strasse = string.Empty;
                string plz = string.Empty;
                string ort = string.Empty;
                string geburtstag = string.Empty;
                string email = string.Empty;
                string telefonP = string.Empty;
                string telefonG = string.Empty;
                string mobile = string.Empty;
                string fax = string.Empty;
                string bemerkung = string.Empty;
                string rechnungsDatum = string.Empty;

                foreach (string formData in form.AllKeys)
                {
                    if (!string.IsNullOrWhiteSpace(form[formData]))
                    {
                        if (form[formData] == "Anrede")
                        {
                            anrede = formData;
                        }
                        if (form[formData] == "Vorname")
                        {
                            vorname = formData;
                        }
                        if (form[formData] == "Name")
                        {
                            name = formData;
                        }
                        if (form[formData] == "Name zusatz")
                        {
                            namezusatz = formData;
                        }
                        if (form[formData] == "Strasse")
                        {
                            strasse = formData;
                        }
                        if (form[formData] == "Plz")
                        {
                            plz = formData;
                        }
                        if (form[formData] == "Ort")
                        {
                            ort = formData;
                        }
                        if (form[formData] == "Geburtstag")
                        {
                            geburtstag = formData;
                        }
                        if (form[formData] == "E-Mail")
                        {
                            email = formData;
                        }
                        if (form[formData] == "Telefon Privat")
                        {
                            telefonP = formData;
                        }
                        if (form[formData] == "Telefon Geschäft")
                        {
                            telefonG = formData;
                        }
                        if (form[formData] == "Mobile")
                        {
                            mobile = formData;
                        }
                        if (form[formData] == "Fax")
                        {
                            fax = formData;
                        }
                        if (form[formData] == "Bemerkung")
                        {
                            bemerkung = formData;
                        }
                        if (form[formData] == "RechnungsDatum")
                        {
                            rechnungsDatum = formData;
                        }
                    }
                }
                List<KeyValueModel> anreden = Utilitys.GetAnreden();
                List<MitgliedModel> list = new List<MitgliedModel>();

                using (ICsvParser csvParser = new CsvParser(new StreamReader(updPath, Encoding.UTF7)))
                {
                    using (CsvReader csv = new CsvReader(csvParser))
                    {
                        csv.Configuration.Encoding = Encoding.UTF8;
                        csv.Configuration.Delimiter = ";";
                        csv.Read();

                        while (csv.Read())
                        {
                            MitgliedModel m = new MitgliedModel();

                            if (!string.IsNullOrWhiteSpace(anrede))
                            {
                                foreach (KeyValueModel a in anreden)
                                {
                                    if (a.Value == csv.GetField<string>(anrede).Trim())
                                    {
                                        m.AnredeId = int.Parse(a.Id);
                                    }
                                }
                            }
                            m.Anrede = string.IsNullOrWhiteSpace(anrede) ? anrede : csv.GetField<string>(anrede);
                            m.Name = string.IsNullOrWhiteSpace(name) ? name : csv.GetField<string>(name);
                            m.ZusatzName = string.IsNullOrWhiteSpace(namezusatz)
                                ? namezusatz
                                : csv.GetField<string>(namezusatz);
                            m.Vorname = string.IsNullOrWhiteSpace(vorname) ? vorname : csv.GetField<string>(vorname);
                            m.Strasse = string.IsNullOrWhiteSpace(strasse) ? strasse : csv.GetField<string>(strasse);
                            m.Plz = string.IsNullOrWhiteSpace(plz) ? plz : csv.GetField<string>(plz);
                            m.Ort = string.IsNullOrWhiteSpace(ort) ? ort : csv.GetField<string>(ort);
                            m.Mail = string.IsNullOrWhiteSpace(email) ? email : csv.GetField<string>(email);
                            m.TelefonP = string.IsNullOrWhiteSpace(telefonP) ? telefonP : csv.GetField<string>(telefonP);
                            m.TelefonG = string.IsNullOrWhiteSpace(telefonG) ? telefonG : csv.GetField<string>(telefonG);
                            m.Fax = string.IsNullOrWhiteSpace(fax) ? fax : csv.GetField<string>(fax);
                            m.Mobile = string.IsNullOrWhiteSpace(mobile) ? mobile : csv.GetField<string>(mobile);

                            m.Geburtstag = string.IsNullOrWhiteSpace(geburtstag)
                                ? geburtstag
                                : csv.GetField<string>(geburtstag);


                            m.Bemerkung = string.IsNullOrWhiteSpace(bemerkung)
                                ? bemerkung
                                : csv.GetField<string>(bemerkung);

                            DateTime rd;
                            if (string.IsNullOrWhiteSpace(rechnungsDatum))
                            {
                                rd = DateTime.Now.Date;
                            }
                            else
                            {
                                if (!DateTime.TryParse(csv.GetField<string>(rechnungsDatum), out rd))
                                {
                                    rd = DateTime.Now.Date;
                                }
                            }

                            m.RechnungsDatum = rd;

                            m.MandantId = session.MandantId;

                            m.MandantBenutzerGruppeId = 0;

                            list.Add(m);
                        }
                    }
                }

                Mitglieder mitglieder = new Mitglieder();
                mitglieder.DelMitgliederByMandantId(session);

                totalMitglieder = list.Count;

                foreach (var item in list)
                {
                    if (!mitglieder.ImportMitglied(item, out errorItem, out errorMessage))
                    {
                        totalError += 1;

                        MitgliedImportErrorViewModel m = new MitgliedImportErrorViewModel
                        {
                            Anrede = errorItem.Anrede,
                            Vorname = errorItem.Vorname,
                            Name = errorItem.Name,
                            Strasse = errorItem.Strasse,
                            Plz = errorItem.Plz,
                            Ort = errorItem.Ort,
                            Mail = errorItem.Mail,
                            TelefonG = errorItem.TelefonG,
                            TelefonP = errorItem.TelefonP,
                            Mobile = errorItem.Mobile,
                            Fax = errorItem.Fax,
                            Geburtstag = errorItem.Geburtstag,
                            ZusatzName = errorItem.ZusatzName,
                            Bemerkung = errorItem.Bemerkung,
                            FehlerNachricht = errorMessage
                        };

                        importModel.Add(m);
                    }
                }

                if (System.IO.File.Exists(updPath))
                {
                    try
                    {
                        System.IO.File.Delete(updPath);
                    }
                    catch (Exception ex)
                    {
                        Log.Net.Error("Mitglieder Import Upload File konnte nicht gelöscht werden! Pfad(" + updPath +
                                      ": " + ex);
                    }
                }
            }

            Session["CsvFile"] = null;

            MitgliedImportCounterViewModel importCounterModel = new MitgliedImportCounterViewModel();
            importCounterModel.TotalImport = totalMitglieder - totalError;
            importCounterModel.TotalFehler = totalError;
            importCounterModel.MitgliederFehler = importModel;

            Session["ImportUebersicht"] = importCounterModel;

            return Json(true);
        }

        public ActionResult ImportUebersicht()
        {
            MitgliedImportCounterViewModel importCounterModel =
                (MitgliedImportCounterViewModel) Session["ImportUebersicht"];
            return PartialView("_ImportUebersicht", importCounterModel);
        }

        public ActionResult Zuortnen()
        {
            if (Session["MandantSession"] != null
                && Session["CsvFile"] != null)
            {
                string updPath = Session["CsvFile"].ToString();

                FelderZuordnerViewModel model = new FelderZuordnerViewModel();

                using (ICsvParser csvParser = new CsvParser(new StreamReader(updPath, Encoding.UTF7)))
                {
                    using (CsvReader csv = new CsvReader(csvParser))
                    {
                        csv.Configuration.Encoding = Encoding.UTF8;
                        csv.Configuration.Delimiter = ";";
                        csv.Read();

                        List<string> headerList = new List<string>();
                        for (int i = 0; i < csv.FieldHeaders.Length; i++)
                        {
                            headerList.Add(csv.FieldHeaders[i].Trim());
                        }

                        model.FeldNamen = headerList;

                        List<string> drp = new List<string>();
                        drp.Add("-- Nicht zuweisen --");
                        drp.Add("Anrede");
                        drp.Add("Vorname");
                        drp.Add("Name");
                        drp.Add("Name zusatz");
                        drp.Add("Strasse");
                        drp.Add("Plz");
                        drp.Add("Ort");
                        drp.Add("Geburtstag");
                        drp.Add("E-Mail");
                        drp.Add("Telefon Privat");
                        drp.Add("Telefon Geschäft");
                        drp.Add("Mobile");
                        drp.Add("Fax");
                        drp.Add("Bemerkung");
                        drp.Add("RechnungsDatum");
                        drp.Sort();
                        model.SelectFeldName = drp;
                    }
                }


                return PartialView("_Zuortnen", model);
            }

            return PartialView("_Upload");
        }

        public JsonResult DelMitglieder()
        {
            RequestMessage message = new RequestMessage();
            if (Session["CsvFile"] != null
                && Session["MandantSession"] != null)
            {
                MandantSession session = (MandantSession) Session["MandantSession"];
                Mitglieder mitglieder = new Mitglieder();
                if (mitglieder.DelMitgliederByMandantId(session))
                {
                    message.IsError = false;
                    message.Message = "Mitglieder erfolgreich gelöscht.";
                    return Json(message);
                }
            }
            message.IsError = true;
            message.Message = "Fehler beim versuch die Mitglieder zu löschen.";
            return Json(message);
        }

        public JsonResult ImportMitglieder()
        {
            RequestMessage message = new RequestMessage();
            List<MitgliedModel> list = Session["CsvFile"] as List<MitgliedModel>;

            if (list.Count > 0)
            {
                Mitglieder mitglieder = new Mitglieder();
                foreach (var item in list)
                {
                    mitglieder.SetMitglied(item);
                }

                message.IsError = false;
                message.Message = "Mitglieder erfolgreich importiert.";
                return Json(message);
            }

            message.IsError = true;
            message.Message = "Fehler beim versuch die Mitglieder zu importieren.";
            return Json(message);
        }

        public ActionResult Upload()
        {
            return PartialView("_Upload");
        }
    }
}