namespace Models
{
    using System;

    public class MitgliedModel
    {
        public int MitgliedId { get; set; }
        public int MandantId { get; set; }
        public int AnredeId { get; set; }
        public string Anrede { get; set; }
        public string Vorname { get; set; }
        public string Name { get; set; }
        public string ZusatzName { get; set; }
        public string Strasse { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Geburtstag { get; set; }
        public string Mail { get; set; }
        public string TelefonP { get; set; }
        public string TelefonG { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public int MitgliedschaftTypeId { get; set; }
        public string MitgliedschaftTypeName { get; set; }
        public int MandantBenutzerGruppeId { get; set; }
        public string MandantBenutzerGruppeName { get; set; }
        public DateTime RechnungsDatum { get; set; }
        public string RechnungsDatumString { get; set; }
        public string Bemerkung { get; set; }
        public int LandId { get; set; }
        public string LandName { get; set; }
        public bool Selected { get; set; }
    }
}
