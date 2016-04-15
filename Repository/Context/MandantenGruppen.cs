namespace Repository.Context
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models;
    using Data;
    using VereinDataRoot;
    public static class MandantenGruppen
    {
        private static VereinDBEntities _entities;

        public static List<KeyValueModel> GetMandantGruppenForAll(int mandantId)
        {
            try
            {
                List<KeyValueModel> list = new List<KeyValueModel>();

                using (_entities = new VereinDBEntities())
                {
                    IQueryable<MandantenBenutzerGruppen> l = (from m in _entities.MandantenBenutzerGruppens
                                                              where m.MandantId == mandantId || m.MandantId == 0
                                                              orderby m.MandantBenutzerGruppeName ascending
                                                              select m);

                    foreach (MandantenBenutzerGruppen item in l)
                    {
                        KeyValueModel m = new KeyValueModel();
                        m.Value = item.MandantBenutzerGruppeName.Trim();
                        m.Id = item.MandantBenutzerGruppeId.ToString();
                        list.Add(m);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MandantenGruppen GetMandantGruppen: " + ex);
                return new List<KeyValueModel>();
            }
        }

        public static List<KeyValueModel> GetMandantGruppen(int mandantId)
        {
            try
            {
                List<KeyValueModel> list = new List<KeyValueModel>();

                using (_entities = new VereinDBEntities())
                {
                    IQueryable<MandantenBenutzerGruppen> l = (from m in _entities.MandantenBenutzerGruppens
                                                              where m.MandantId == mandantId
                                                              orderby m.MandantBenutzerGruppeName ascending
                                                              select m);

                    foreach (MandantenBenutzerGruppen item in l)
                    {
                        KeyValueModel m = new KeyValueModel();
                        m.Value = item.MandantBenutzerGruppeName.Trim();
                        m.Id = item.MandantBenutzerGruppeId.ToString();
                        list.Add(m);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MandantenGruppen GetMandantGruppen: " + ex);
                return new List<KeyValueModel>();
            }
        }

        public static bool SetMandantGruppe(MandantGruppe model)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    MandantenBenutzerGruppen l = (from m in _entities.MandantenBenutzerGruppens
                                                where m.MandantId == model.MandantId
                                                && m.MandantBenutzerGruppeId == model.MandantBenutzerGruppeId
                                                select m).FirstOrDefault();

                    if (l == null)
                    {
                        l = new MandantenBenutzerGruppen();
                        l.MandantId = model.MandantId;
                        l.MandantBenutzerGruppeName = model.MandantBenutzerGruppeName.Trim();
                        _entities.MandantenBenutzerGruppens.Add(l);
                    }
                    else
                    {
                        l.MandantBenutzerGruppeName = model.MandantBenutzerGruppeName.Trim();
                    }

                    _entities.SaveChanges();
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MandantenGruppen SetMandantGruppe: " + ex);
                return false;
            }
        }

        public static bool DelMandantGruppe(MandantGruppe model)
        {
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    MandantenBenutzerGruppen l = (from m in _entities.MandantenBenutzerGruppens
                                                  where m.MandantId == model.MandantId
                                                  && m.MandantBenutzerGruppeId == model.MandantBenutzerGruppeId
                                                  select m).FirstOrDefault();

                    if (l != null)
                    {
                        _entities.MandantenBenutzerGruppens.Remove(l);
                        _entities.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class MandantenGruppen SetMandantGruppe: " + ex);
                return false;
            }
        }
    }
}
