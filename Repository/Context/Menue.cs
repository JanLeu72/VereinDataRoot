using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Repository.Data;
using VereinDataRoot;

namespace Repository.Context
{
    public static class Menue
    {
        private static VereinDataRootDBEntities _entities;

        public static List<TreeNavigationModel> GetMainNavigation()
        {
            List<TreeNavigationModel> list = new List<TreeNavigationModel>();
            using (_entities = new VereinDataRootDBEntities())
            {
                IQueryable<TreeNavigation> items = (from n in _entities.TreeNavigations
                                                    where n.Aktiv
                                                    orderby n.Sort
                                                    select n);

                foreach (TreeNavigation item in items)
                {
                    TreeNavigationModel t = new TreeNavigationModel();
                    t.TreeId = item.TreeId;
                    t.TreeNavigationId = item.TreeNavigationId;
                    t.DisplayName = item.DisplayNameDe.Trim();
                    t.ActionMvc = item.Action.Trim();
                    t.ControllerMvc = item.Controller.Trim();
                    list.Add(t);
                }
            }

            return list;

        }

        public static List<MandantenBenutzerNavigation> GetMandantenBenutzerNavigation(int benutzerId, int mandantId)
        {
            try
            {
                List<MandantenBenutzerNavigation> list = new List<MandantenBenutzerNavigation>();
                using (_entities = new VereinDataRootDBEntities())
                {
                    var items = (from m in _entities.MandantenBenutzerModule_Benutzer
                                 where m.tblBenutzer.BenutzerId == benutzerId
                                       && m.tblModule.tblMandantenModules.Any(mm => mm.MandantId == mandantId)
                                 orderby m.tblModule.Sort
                                 select m);

                    foreach (MandantenBenutzerModule_Benutzer item in items)
                    {
                        MandantenBenutzerNavigation model = new MandantenBenutzerNavigation();
                        model.ModulName = item.tblModule.ModulName.Trim();
                        model.ControlerName = item.tblModule.ControlerName.Trim();
                        model.ActionName = item.tblModule.ActionName.Trim();

                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                Log.Net.Error("GetMandantenBenutzerNavigation: " + ex);
                return null;
            }
        }
    }
}
