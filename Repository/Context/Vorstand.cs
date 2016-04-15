using Models;
using Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using VereinDataRoot;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Context
{
    public static class Vorstand
    {
        private static VereinDBEntities _entities;

        public static bool DelVorstandsMitglied(int vorstandId, int mitgliedId, int ressortId, int mandantId)
        {
            bool ok = false;
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    RessortMandantMitglieder item = (from r in _entities.RessortMandantMitglieders
                                                     where r.MandantId == mandantId
                                                     && r.MitgliedId == mitgliedId
                                                     && r.RessortId == ressortId
                                                     && r.RessortMandantMitgliedId == vorstandId
                                                     select r).FirstOrDefault();

                    if (item != null)
                    {
                        _entities.RessortMandantMitglieders.Remove(item);
                        _entities.SaveChanges();

                        ok = true;
                    }
                    else
                    {
                        ok = false;
                    }
                }

                return ok;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Vorstand DelVorstandsMitglied: " + ex);
                return false;
            }
        }
        public static bool SetVorstandsMitglied(int mitgliedId, int ressortId, int mandantId)
        {
            bool ok = false;
            try
            {
                using (_entities = new VereinDBEntities())
                {
                    RessortMandantMitglieder item = (from r in _entities.RessortMandantMitglieders
                                                     where r.MandantId == mandantId
                                                     && r.RessortId == ressortId
                                                     && r.MitgliedId == mitgliedId
                                                     select r).FirstOrDefault();

                    if (item == null)
                    {
                        item = new RessortMandantMitglieder();
                        item.MandantId = mandantId;
                        item.RessortId = ressortId;
                        item.MitgliedId = mitgliedId;

                        _entities.RessortMandantMitglieders.Add(item);
                        _entities.SaveChanges();

                        ok = true;
                    }
                    else
                    {
                        ok = false;
                    }
                }

                return ok;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class Vorstand SetVorstandsMitglied: " + ex);
                return false;
            }
        }

        public static List<VorstandModel> GetVorstaende(int mandantId)
        {
            List<VorstandModel> list = new List<VorstandModel>();

            using (_entities = new VereinDBEntities())
            {
                IQueryable<RessortMandantMitglieder> items = (from r in _entities.RessortMandantMitglieders
                                            where r.MandantId == mandantId
                                            orderby r.Ressort.Sort
                                            select r);

                foreach (RessortMandantMitglieder item in items)
                {
                    VorstandModel vm = new VorstandModel();
                    vm.VorstandId = item.RessortMandantMitgliedId;
                    vm.MitgliedId = item.MitgliedId;
                    vm.RessortName = item.Ressort.RessortName.Trim();
                    vm.RessortId = item.RessortId;
                    vm.MitgliedAnrede = item.Mitglieder.Anrede.AnredeName.Trim();
                    vm.MitgliedName = item.Mitglieder.Nachname.Trim();
                    vm.MitgliedVorname = item.Mitglieder.Vorname.Trim();
                    vm.MitgliedPlz = item.Mitglieder.Plz.Trim();
                    vm.MitgliedStrasse = item.Mitglieder.Strasse.Trim();
                    vm.MitgliedOrt = item.Mitglieder.Ort.Trim();
                    list.Add(vm);
                }
            }

            return list;
        }
    }
}
