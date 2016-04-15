using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Models.Backup
{
    [XmlRoot("VereinBackup", Namespace = "http://www.dataroot.ch", IsNullable = false)]
    public class VereinBackup
    {
        [XmlElement("MandantId")]
        public int MandantId { get; set; }

        [XmlArray("Mitglieder")]
        [XmlArrayItem("Mitglied")]
        public List<MitgliedBackup> Mitglieder { get; set; } 
    }
}
