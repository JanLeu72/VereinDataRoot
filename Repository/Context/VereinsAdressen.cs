namespace Repository.Context
{
    using System;
    using System.Linq;
    using Models;
    using Data;
    using VereinDataRoot;

    public static class VereinsAdressen
    {
        private static VereinDataRootDBEntities _entities;

        public static VereinAdresse GetVereinAdresse(int mandantId, int mandantAdressTypeId)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    MandantenAdressen item = (from a in _entities.MandantenAdressens
                                              where a.MandantId == mandantId
                                              && a.MandantAdressTypeId == mandantAdressTypeId
                                              select a).FirstOrDefault();

                    if (item != null)
                    {
                        VereinAdresse model = new VereinAdresse();
                        model.VereinAdresseId = item.MandantAdressId;
                        model.MandantId = item.MandantId;
                        model.MandantAdressTypeId = item.MandantAdressTypeId;
                        model.Standart = item.Standart;
                        model.Anrede = item.Anrede.Trim();
                        model.Name = item.Name.Trim();
                        model.Vorname = item.Vorname.Trim();
                        model.Strasse = item.Strasse.Trim();
                        model.Plz = item.Plz.Trim();
                        model.Ort = item.Ort.Trim();
                        model.Postfach = item.Postfach.Trim();
                        model.AdresseZusatz = item.AdresseZusatz.Trim();
                        model.NameZusatz = item.NameZusatz.Trim();
                        model.Mail = item.Mail.Trim();

                        return model;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class VereinsAdressen GetVereinAdresse: " + ex);
                return null;
            }
        }

        public static bool SetAdresse(int mandantId, int mandantAdressTypeId, VereinAdresse adresse)
        {
            try
            {
                using (_entities = new VereinDataRootDBEntities())
                {
                    MandantenAdressen item = (from a in _entities.MandantenAdressens
                                              where a.MandantId == mandantId
                                              && a.MandantAdressTypeId == mandantAdressTypeId
                                              select a).FirstOrDefault();

                    if (item != null)
                    {
                        item.Name = string.IsNullOrWhiteSpace(adresse.Name) ? string.Empty : adresse.Name.Trim();
                        item.Vorname = string.IsNullOrWhiteSpace(adresse.Vorname)? string.Empty: adresse.Vorname.Trim();
                        item.Strasse = string.IsNullOrWhiteSpace(adresse.Strasse)
                            ? string.Empty
                            : adresse.Strasse.Trim();
                        item.Plz = string.IsNullOrWhiteSpace(adresse.Plz) ? string.Empty : adresse.Plz.Trim();
                        item.Ort = string.IsNullOrWhiteSpace(adresse.Ort) ? string.Empty : adresse.Ort.Trim();
                        item.Postfach = string.IsNullOrWhiteSpace(adresse.Postfach)
                            ? string.Empty
                            : adresse.Postfach.Trim();
                        item.Anrede = string.IsNullOrWhiteSpace(adresse.Anrede) ? string.Empty : adresse.Anrede.Trim();
                        item.AdresseZusatz = string.IsNullOrWhiteSpace(adresse.AdresseZusatz)
                            ? string.Empty
                            : adresse.AdresseZusatz.Trim();
                        item.Mail = string.IsNullOrWhiteSpace(adresse.Mail) ? string.Empty : adresse.Mail.Trim();
                        item.NameZusatz = string.IsNullOrWhiteSpace(adresse.NameZusatz)
                            ? string.Empty
                            : adresse.NameZusatz.Trim();
                        item.Standart = false;
                    }
                    else
                    {
                        item = new MandantenAdressen
                        {
                            MandantId = mandantId,
                            MandantAdressTypeId = mandantId,
                            Name = string.IsNullOrWhiteSpace(adresse.Name) ? string.Empty : adresse.Name.Trim(),
                            Vorname = string.IsNullOrWhiteSpace(adresse.Vorname) ? string.Empty: adresse.Vorname.Trim(),
                            Strasse = string.IsNullOrWhiteSpace(adresse.Strasse) ? string.Empty : adresse.Strasse.Trim(),
                            Plz = string.IsNullOrWhiteSpace(adresse.Plz) ? string.Empty : adresse.Plz.Trim(),
                            Ort = string.IsNullOrWhiteSpace(adresse.Ort) ? string.Empty : adresse.Ort.Trim(),
                            Postfach = string.IsNullOrWhiteSpace(adresse.Postfach) ? string.Empty : adresse.Postfach.Trim(),
                            Anrede = string.IsNullOrWhiteSpace(adresse.Anrede) ? string.Empty : adresse.Anrede.Trim(),
                            AdresseZusatz = string.IsNullOrWhiteSpace(adresse.AdresseZusatz) ? string.Empty : adresse.AdresseZusatz.Trim(),
                            Mail = string.IsNullOrWhiteSpace(adresse.Mail) ? string.Empty : adresse.Mail.Trim(),
                            NameZusatz = string.IsNullOrWhiteSpace(adresse.NameZusatz) ? string.Empty : adresse.NameZusatz.Trim(),
                            Standart = false
                        };
                        _entities.MandantenAdressens.Add(item);
                    }

                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Net.Error("class VereinsAdressen SetVereinAdresse: " + ex);
                return false;
            }
        }
    }
}
