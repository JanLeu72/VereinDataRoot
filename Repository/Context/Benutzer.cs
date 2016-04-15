namespace Repository.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Data;
    using VereinDataRoot;

    public class Benutzer
    {
        private static VereinDataRootDBEntities _entities;

        public MandantSession GetCrossLogin(int benutzerId)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    Benutzer_Benutzer item = (from m in _entities.Benutzer_Benutzer
                                              where m.BenutzerId == benutzerId
                                              select m).FirstOrDefault();

                    if (item != null)
                    {
                        MandantSession session = new MandantSession();
                        session.BenutzerName = item.BenutzerName.Trim();
                        session.BenutzerId = item.BenutzerId;
                        session.MandantName = item.tblMandanten.MandantName.Trim();
                        session.MandantId = item.MandantId;
                        session.PageStyle = item.PageStyle;

                        return session;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer GetCrossLogin: " + ex);
                return null;
            }
        }

        public MandantSession GetBenutzerLogin(BenutzerModel model)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    Benutzer_Benutzer item = (from m in _entities.Benutzer_Benutzer
                        where m.Mail == model.BenutzerMail
                        && m.Passwort == model.Passwort
                        select m).FirstOrDefault();

                    if (item != null)
                    {
                        item.LetzteAnmeldung = DateTime.Now;
                        _entities.SaveChanges();

                        MandantSession session = new MandantSession();
                        session.BenutzerName = item.BenutzerName.Trim();
                        session.BenutzerId = item.BenutzerId;
                        session.MandantName = item.tblMandanten.MandantName.Trim();
                        session.MandantId = item.MandantId;
                        session.PageStyle = item.PageStyle;

                        return session;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer GetBenutzerLogin: " + ex);
                return null;
            }
        }

        public static List<BenutzerModel> GetBenutzers(int mandantId)
        {
            try
            {
                List<BenutzerModel> list = new List<BenutzerModel>();
                using (_entities = new VereinDataRootDBEntities())
                {
                    IQueryable<Benutzer_Benutzer> items = (from b in _entities.Benutzer_Benutzer
                                                     where b.MandantId == mandantId
                                                     orderby b.BenutzerName
                                                     select b);

                    foreach (Benutzer_Benutzer item in items)
                    {
                        BenutzerModel model = new BenutzerModel();
                        model.BenutzerId = item.BenutzerId;
                        model.BenutzerName = item.BenutzerName.Trim();
                        model.BenutzerMail = item.Mail.Trim();
                        model.Aktive = item.Aktiv;
                        model.ErstelltAm = item.Erstellt;
                        model.LetztesLogin = item.LetzteAnmeldung;
                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer GetBenutzers: " + ex);
                return null;
            }
        }

        public static bool SetBenutzer(BenutzerModel model)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    var item = (from b in _entities.Benutzer_Benutzer
                                where b.BenutzerId == model.BenutzerId
                                && b.MandantId == model.MandantId
                                select b).FirstOrDefault();

                    if (item == null)
                    {
                        Benutzer_Benutzer benutzer = new Benutzer_Benutzer()
                        {
                            BenutzerName = model.BenutzerName,
                            Mail = model.BenutzerMail,
                            Passwort = model.Passwort,
                            Aktiv = model.Aktive,
                            Erstellt = DateTime.Now,
                            LetzteAnmeldung = DateTime.Now,
                            MandantId = model.MandantId,
                            PageStyle = "bootstrap"
                        };

                        _entities.Benutzer_Benutzer.Add(benutzer);
                    }
                    else
                    {
                        item.BenutzerName = model.BenutzerName;
                        item.Mail = model.BenutzerMail;
                        item.Aktiv = model.Aktive;
                    }

                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer SetBenutzer: " + ex);
                return false;
            }
        }

        public static bool DelBenutzer(BenutzerModel model)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    var item = (from b in _entities.Benutzer_Benutzer
                                where b.BenutzerId == model.BenutzerId
                                && b.MandantId == model.MandantId
                                select b).FirstOrDefault();

                    if (item != null)
                    {
                        _entities.Benutzer_Benutzer.Remove(item);
                        _entities.SaveChanges();
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer DelBenutzer: " + ex);
                return false;
            }
        }

        public static List<MandantBenutzerModul> GetBenutzerMandantModule(int benunterId, int mandantId)
        {
            try
            {
                List<MandantBenutzerModul> list = new List<MandantBenutzerModul>();
                using (_entities = new VereinDataRootDBEntities())
                {
                    IQueryable<Module_Benutzer> module = (from b in _entities.Module_Benutzer
                                                    where b.tblMandantenModules.Any(m => m.MandantId == mandantId)
                                                    orderby b.ModulName
                                                    select b);

                    IQueryable<MandantenBenutzerModule_Benutzer> items = (from b in _entities.MandantenBenutzerModule_Benutzer
                                                                    where b.BenutzerId == benunterId
                                                                    orderby b.tblModule.ModulName
                                                                    select b);

                    foreach (Module_Benutzer modul in module)
                    {
                        MandantBenutzerModul model = new MandantBenutzerModul();
                        model.ModulId = modul.ModulId;
                        model.ModulName = modul.ModulName.Trim();
                        model.IsSelectet = items.Any(m => m.BenutzerId == benunterId && m.ModulId == modul.ModulId);
                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer GetBenutzerMandantModule: " + ex);
                return null;
            }
        }

        public static bool SetBenutzerModul(int benutzerId, int modulId, bool check)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {

                    MandantenBenutzerModule_Benutzer item = (from b in _entities.MandantenBenutzerModule_Benutzer
                                                       where b.BenutzerId == benutzerId
                                                       && b.ModulId == modulId
                                                       orderby b.tblModule.ModulName
                                                       select b).FirstOrDefault();
                    if (item != null)
                    {
                        if (!check)
                        {
                            _entities.MandantenBenutzerModule_Benutzer.Remove(item);
                        }
                    }
                    else
                    {
                        if (check)
                        {
                            MandantenBenutzerModule_Benutzer model = new MandantenBenutzerModule_Benutzer();
                            model.ModulId = modulId;
                            model.BenutzerId = benutzerId;
                            _entities.MandantenBenutzerModule_Benutzer.Add(model);
                        }
                    }

                    _entities.SaveChanges();

                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer SetBenutzerModul: " + ex);
                return false;
            }
        }

        public static bool SetBenutzerPasswort(int benunterId, int mandantId, string pw)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    var item = (from b in _entities.Benutzer_Benutzer
                                where b.BenutzerId == benunterId
                                && b.MandantId == mandantId
                                select b).FirstOrDefault();

                    if (item != null)
                    {
                        item.Passwort = pw.Trim();
                    }

                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Benutzer SetBenutzerPasswort: " + ex);
                return false;
            }
        }
    }
}
