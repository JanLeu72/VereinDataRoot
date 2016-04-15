namespace Repository.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using LinqKit;
    using Models;
    using Data;
    using VereinDataRoot;

    public class Mitglieder
    {
        private VereinDBEntities _entities;

        public List<VorstandModel> GetSearchMitglieder(string searchText, int mandantId)
        {
            List<VorstandModel> list = new List<VorstandModel>();

            using (_entities = new VereinDBEntities())
            {
                IQueryable<Mitglied> l;
                l = (from m in _entities.Mitglieds
                     where m.MandantId == mandantId
                     && m.Vorname.StartsWith(searchText)
                     orderby m.Vorname ascending
                     select m).Skip(0).Take(10);

                foreach (Mitglied item in l)
                {
                    VorstandModel model = new VorstandModel
                    {
                        MitgliedId = item.MitgliedId,
                        MitgliedAnrede = item.Anrede.AnredeName,
                        MitgliedName = item.Nachname,
                        MitgliedVorname = item.Vorname,
                        MitgliedStrasse = item.Strasse,
                        MitgliedPlz = item.Plz,
                        MitgliedOrt = item.Ort,
                        VorstandName = item.Nachname.Trim() + " " + item.Vorname.Trim() + " " + item.Ort.Trim(),
                        RessortId = 0
                    };

                    list.Add(model);
                }
            }

            return list;
        }

        public bool DelMitgliedById(MitgliedModel model)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    Mitglied item = (from m in _entities.Mitglieds
                            where m.MitgliedId == model.MitgliedId
                                && m.MandantId == model.MandantId
                            select m).FirstOrDefault();

                    if (item != null)
                    {
                        _entities.Mitglieds.Remove(item);
                        _entities.SaveChanges();

                        return true;
                    }
                    Log.Net.Error("class Mitglieder DelMitgliedById: Mitglied konnte nicht gelöscht werden! MandatnId: " + model.MandantId + " / MitgliedId: " + model.MitgliedId );
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder DelMitgliedById: " + ex);
                return false;
            }
        }

        public bool SetMitglied(MitgliedModel model)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    Mitglied item = (from m in _entities.Mitglieds
                        where m.MitgliedId == model.MitgliedId
                              && m.MandantId == model.MandantId
                        select m).FirstOrDefault();

                    if (item != null)
                    {
                        item.AnredeId = model.AnredeId;
                        item.Nachname = model.Name;
                        item.Vorname = model.Vorname;
                        item.ZusatzName = model.ZusatzName;
                        item.Strasse = model.Strasse;
                        item.Plz = model.Plz;
                        item.Ort = model.Ort;
                        item.Mail = model.Mail;
                        item.TelefonP = model.TelefonP;
                        item.TelefonG = model.TelefonG;
                        item.Mobile = model.Mobile;
                        item.Fax = model.Fax;
                        item.Geburtstag = model.Geburtstag;
                        item.MitgliedschaftTypeId = model.MitgliedschaftTypeId;
                        item.RechnungsDatum = model.RechnungsDatum < DateTime.Now.AddYears(-200)
                            ? DateTime.Now
                            : model.RechnungsDatum;
                        item.Bemerkung = model.Bemerkung.Trim();
                        item.MandantBenuterGruppeId = model.MandantBenutzerGruppeId;
                        item.LandId = model.LandId;
                    }
                    else
                    {
                        item = new Mitglied
                        {
                            AnredeId = model.AnredeId,
                            Nachname = model.Name,
                            Vorname = model.Vorname,
                            ZusatzName = model.ZusatzName,
                            Strasse = model.Strasse,
                            Plz = model.Plz,
                            Ort = model.Ort,
                            Mail = model.Mail,
                            TelefonP = model.TelefonP,
                            TelefonG = model.TelefonG,
                            Mobile = model.Mobile,
                            Fax = model.Fax,
                            MandantId = model.MandantId,
                            Geburtstag = model.Geburtstag,
                            MitgliedschaftTypeId = model.MitgliedschaftTypeId,
                            RechnungsDatum =
                                model.RechnungsDatum == DateTime.Parse("01.01.0001 00:00:00")
                                    ? DateTime.Now
                                    : model.RechnungsDatum,
                            Bemerkung = model.Bemerkung.Trim(),
                            MandantBenuterGruppeId = model.MandantBenutzerGruppeId,
                            LandId = model.LandId
                        };
                        _entities.Mitglieds.Add(item);
                    }
                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder SetMitglied: " + ex);
                return false;
            }
        }

        public bool ImportMitglied(MitgliedModel model, out MitgliedModel mitglied, out string message)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    Mitglied item = new Mitglied
                    {
                        AnredeId = model.AnredeId,
                        Nachname = model.Name,
                        Vorname = model.Vorname,
                        ZusatzName = model.ZusatzName,
                        Strasse = model.Strasse,
                        Plz = model.Plz,
                        Ort = model.Ort,
                        Mail = model.Mail,
                        TelefonP = model.TelefonP,
                        TelefonG = model.TelefonG,
                        Mobile = model.Mobile,
                        Fax = model.Fax,
                        MandantId = model.MandantId,
                        Geburtstag = model.Geburtstag,
                        MitgliedschaftTypeId = model.MitgliedschaftTypeId,
                        RechnungsDatum =
                            model.RechnungsDatum == DateTime.Parse("01.01.0001 00:00:00")
                                ? DateTime.Now
                                : model.RechnungsDatum,
                        Bemerkung = model.Bemerkung.Trim(),
                        MandantBenuterGruppeId = model.MandantBenutzerGruppeId,
                        LandId = model.LandId
                    };
                    _entities.Mitglieds.Add(item);
                    _entities.SaveChanges();
                    message = string.Empty;
                    mitglied = new MitgliedModel();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder ImportMitglied: " + ex);
                message = ex.Message;
                mitglied = model;
                return false;
            }
        }

        public MitgliedGrid GetMitglieder(MitgliedRequest request)
        {
            try
            {
                List<MitgliedModel> list = new List<MitgliedModel>();
                int anzahl = 0;

                using (_entities = new VereinDBEntities())
                {
                    var predicate = PredicateBuilder.True<Mitglied>();
                    predicate = predicate.And(p => p.MandantId == request.MandantId);

                    if (request.Filters.Count > 0)
                    {
                        if (request.Filters[0].Field == "Anrede")
                        {
                            if (request.Filters[0].Operator == "eq")
                            {
                                int id;
                                int.TryParse(request.Filters[0].Value, out id);

                                predicate = predicate.And(p => p.AnredeId == id);
                            }
                        }

                        if (request.Filters[0].Field == "Vorname")
                        {
                            string s = request.Filters[0].Value;
                            if (request.Filters[0].Operator == "eq")
                            {
                                predicate = predicate.And(p => p.Vorname.Equals(s));
                            }

                            if (request.Filters[0].Operator == "startswith")
                            {
                                predicate = predicate.And(p => p.Vorname.StartsWith(s));
                            }
                        }

                        if (request.Filters[0].Field == "Name")
                        {
                            string s = request.Filters[0].Value;
                            if (request.Filters[0].Operator == "eq")
                            {
                                predicate = predicate.And(p => p.Nachname.Equals(s));
                            }

                            if (request.Filters[0].Operator == "startswith")
                            {
                                predicate = predicate.And(p => p.Nachname.StartsWith(s));
                            }
                        }

                        if (request.Filters[0].Field == "Strasse")
                        {
                            string s = request.Filters[0].Value;
                            if (request.Filters[0].Operator == "eq")
                            {
                                predicate = predicate.And(p => p.Strasse.Equals(s));
                            }

                            if (request.Filters[0].Operator == "startswith")
                            {
                                predicate = predicate.And(p => p.Strasse.StartsWith(s));
                            }
                        }

                        if (request.Filters[0].Field == "Plz")
                        {
                            string s = request.Filters[0].Value;
                            if (request.Filters[0].Operator == "eq")
                            {
                                predicate = predicate.And(p => p.Plz.Equals(s));
                            }

                            if (request.Filters[0].Operator == "startswith")
                            {
                                predicate = predicate.And(p => p.Plz.StartsWith(s));
                            }
                        }

                        if (request.Filters[0].Field == "Ort")
                        {
                            string s = request.Filters[0].Value;
                            if (request.Filters[0].Operator == "eq")
                            {
                                predicate = predicate.And(p => p.Ort.Equals(s));
                            }

                            if (request.Filters[0].Operator == "startswith")
                            {
                                predicate = predicate.And(p => p.Ort.StartsWith(s));
                            }
                        }

                        if (request.Filters[0].Field == "MitgliedschaftTypeName")
                        {
                            if (request.Filters[0].Operator == "eq")
                            {
                                int id;
                                int.TryParse(request.Filters[0].Value, out id);

                                predicate = predicate.And(p => p.MitgliedschaftTypeId == id);
                            }
                        }

                        if (request.Filters[0].Field == "MandantBenutzerGruppeName")
                        {
                            if (request.Filters[0].Operator == "eq")
                            {
                                int id;
                                int.TryParse(request.Filters[0].Value, out id);

                                predicate = predicate.And(p => p.MandantBenuterGruppeId == id);
                            }
                        }

                        if (request.Filters[0].Field == "RechnungsDatumString")
                        {
                            if (request.Filters[0].Operator == "eq")
                            {
                                DateTime date;
                                DateTime.TryParse(request.Filters[0].Value, out date);

                                predicate = predicate.And(p => p.RechnungsDatum == date.Date);
                            }
                            if (request.Filters[0].Operator == "gte")
                            {
                                DateTime date;
                                DateTime.TryParse(request.Filters[0].Value, out date);

                                predicate = predicate.And(p => p.RechnungsDatum >= date.Date);
                            }
                            if (request.Filters[0].Operator == "lte")
                            {
                                DateTime date;
                                DateTime.TryParse(request.Filters[0].Value, out date);

                                predicate = predicate.And(p => p.RechnungsDatum <= date.Date);
                            }
                        }
                    } 

                    anzahl = (from Mitglied p in _entities.Mitglieds.AsExpandable().Where(predicate)
                        select p).Count();
                    IQueryable<Mitglied> l;
                    if (request.Sorting.Count > 0)
                    {
                        switch (request.Sorting[0].Field)
                        {
                            case "Anrede":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Anrede.AnredeName ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Anrede.AnredeName descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "Vorname":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Vorname ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Vorname descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "Name":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Nachname ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Nachname descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "Strasse":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Strasse ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Strasse descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "Plz":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Plz ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Plz descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "Ort":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Ort ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.Ort descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "MitgliedschaftTypeName":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.MitgliedschaftType.MitgliedschaftTypeName ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.MitgliedschaftType.MitgliedschaftTypeName descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "MandantBenutzerGruppeName":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                         orderby m.MandantenBenutzerGruppen.MandantBenutzerGruppeName ascending
                                         select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                         orderby m.MandantenBenutzerGruppen.MandantBenutzerGruppeName descending
                                         select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            case "RechnungsDatumString":
                                if (request.Sorting[0].Dir == "asc")
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.RechnungsDatum ascending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                else
                                {
                                    l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                        orderby m.RechnungsDatum descending
                                        select m).Skip(request.Skip).Take(request.Take);
                                }
                                break;
                            default:
                                l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                                    orderby m.Nachname ascending
                                    select m).Skip(request.Skip).Take(request.Take);
                                break;
                        }
                    }
                    else
                    {
                        l = (from m in _entities.Mitglieds.AsExpandable().Where(predicate)
                            orderby m.Nachname ascending
                            select m).Skip(request.Skip).Take(request.Take);
                    }

                    foreach (Mitglied item in l)
                    {
                        MitgliedModel model = new MitgliedModel
                        {
                            MitgliedId = item.MitgliedId,
                            MandantId = item.MandantId,
                            Anrede = item.Anrede.AnredeName,
                            AnredeId = item.AnredeId,
                            Name = item.Nachname,
                            Vorname = item.Vorname,
                            ZusatzName = item.ZusatzName,
                            Strasse = item.Strasse,
                            Plz = item.Plz,
                            Ort = item.Ort,
                            MitgliedschaftTypeName = item.MitgliedschaftType.MitgliedschaftTypeName,
                            MitgliedschaftTypeId = item.MitgliedschaftTypeId,
                            MandantBenutzerGruppeId = item.MandantBenuterGruppeId,
                            MandantBenutzerGruppeName = item.MandantenBenutzerGruppen.MandantBenutzerGruppeName.Trim(),
                            Geburtstag = item.Geburtstag,
                            Mail = item.Mail,
                            TelefonP = item.TelefonP,
                            TelefonG = item.TelefonG,
                            Mobile = item.Mobile,
                            Fax = item.Fax,
                            RechnungsDatumString = item.RechnungsDatum.ToShortDateString(),
                            Bemerkung = item.Bemerkung,
                            LandName = item.Laender.LandName.Trim(),
                            LandId = item.LandId
                        };

                        list.Add(model);
                    }
                }

                MitgliedGrid mitglied = new MitgliedGrid();
                mitglied.Mitglieder = list;
                mitglied.Anzahl = anzahl;

                return mitglied;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder GetMitglieder: " + ex);
                return new MitgliedGrid();
            }
        }

        public bool DelMitgliederByMandantId(MandantSession request)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    _entities.Database.ExecuteSqlCommand("DELETE FROM dbo.tblMitglieder WHERE MandantId = " +
                                                         request.MandantId);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder DelMitgliederByMandantId: " + ex);
                return false;
            }
        }

        public bool SetMitgliederToDefaultMandantGroupeId(MandantGruppe model)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    _entities.Database.ExecuteSqlCommand("UPDATE dbo.tblMitglieder SET MandantBenuterGruppeId = 0 WHERE MandantId = " +
                                                         model.MandantId + " AND MandantBenuterGruppeId = " + model.MandantBenutzerGruppeId);
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Mitglieder SetMitgliederToDefaultMandantGroupeId: " + ex);
                return false;
            }
        }

        public bool GetMaxMitglieder(int mandantId, 
            out int maxMandantMitglieder,
            out int istMitglieder,
            out int mitgliederDiverenz)
        {
            maxMandantMitglieder = 0;
            istMitglieder = 0;
            mitgliederDiverenz = 0;

            try
            {
                using (_entities = new VereinDBEntities())
                {
                    Mandant mandant = (from mm in _entities.Mandants
                                       where mm.MandantId == mandantId
                                       && mm.Aktive
                                       select mm).FirstOrDefault();
                    if (mandant != null)
                    {
                        maxMandantMitglieder = mandant.AnzahlMitgliederMax;

                        istMitglieder = (from m in _entities.Mitglieds
                                         where m.MandantId == mandantId
                                         select m).Count();
                    }

                    mitgliederDiverenz = maxMandantMitglieder - istMitglieder;
                }

                if (mitgliederDiverenz > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Log.Net.Error("GetMaxMitglieder: " + ex);
                return false;
            }
        }

        public bool SetNewMandantenAbo(int mandantId, int aboValue)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    Mandant mandant = (from mm in _entities.Mandants
                                       where mm.MandantId == mandantId
                                       && mm.Aktive
                                       select mm).FirstOrDefault();
                    if (mandant != null)
                    {
                        mandant.AnzahlMitgliederMax = aboValue;
                        mandant.ErstelltAm = DateTime.Now;
                        _entities.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("SetNewMandantenAbo: " + ex);
                return false;
            }
        }
    }
}