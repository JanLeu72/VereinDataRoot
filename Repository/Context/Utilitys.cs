using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using Repository.Data;
using VereinDataRoot;

namespace Repository.Context
{
    public static class Utilitys
    {
        private static VereinDBEntities  _entities;

        public static List<KeyValueModel> GetAnreden()
        {
            List<KeyValueModel> list = new List<KeyValueModel>();
            
            using (_entities = new VereinDBEntities())
            {
                IQueryable<Anrede> items = (from n in _entities.Anredes
                                orderby n.Sort
                                select n);

                foreach (Anrede item in items)
                {
                    KeyValueModel kv = new KeyValueModel();
                    kv.Id = item.AnredeId.ToString();
                    kv.Value = item.AnredeName;
                    list.Add(kv);
                }
            }

            return list;
        }

        public static List<KeyValueModel> GetMitgliedschaftTypen()
        {
            List<KeyValueModel> list = new List<KeyValueModel>();

            using (_entities = new VereinDBEntities())
            {
                IQueryable<MitgliedschaftType> items = (from n in _entities.MitgliedschaftTypes
                                            orderby n.Sort
                                            select n);

                foreach (MitgliedschaftType item in items)
                {
                    KeyValueModel kv = new KeyValueModel();
                    kv.Id = item.MitgliedschaftTypeId.ToString();
                    kv.Value = item.MitgliedschaftTypeName;
                    list.Add(kv);
                }
            }

            return list;
        }

        public static List<KeyValueModel> GetLaender()
        {
            List<KeyValueModel> list = new List<KeyValueModel>();

            using (_entities = new VereinDBEntities())
            {
                IQueryable<Laender> items = (from n in _entities.Laenders
                                                        select n);

                foreach (Laender item in items)
                {
                    KeyValueModel kv = new KeyValueModel();
                    kv.Id = item.LandId.ToString();
                    kv.Value = item.LandName;
                    list.Add(kv);
                }
            }

            return list;
        }

        public static List<KeyValueModel> GetRessorts()
        {
            try
            {
                List<KeyValueModel> list = new List<KeyValueModel>();

                using (_entities = new VereinDBEntities())
                {
                    IQueryable<Ressort> l = (from r in _entities.Ressorts
                                             orderby r.RessortName
                                             select r);

                    foreach (Ressort item in l)
                    {
                        KeyValueModel m = new KeyValueModel();
                        m.Value = item.RessortName.Trim();
                        m.Id = item.RessortId.ToString();
                        list.Add(m);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Vorstand GetRessorts: " + ex);
                return new List<KeyValueModel>();
            }
        }
    }
}
