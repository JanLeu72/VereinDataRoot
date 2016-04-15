using System.Xml.Serialization;

namespace Models.Backup
{
    public class MitgliedBackup
    {
        [XmlAttribute]
        public int MitgliedId { get; set; }

        public int MandantId { get; set; }

        public string Nachname { get; set; }

        public string Vorname { get; set; }
    }
}
