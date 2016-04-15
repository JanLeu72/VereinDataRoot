using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BenutzerModel
    {
        public int MandantId { get; set; }
        public int BenutzerId { get; set; }
        public string BenutzerName { get; set; }
        public string BenutzerMail { get; set; }
        public string Passwort { get; set; }
        public bool Aktive { get; set; }
        public DateTime ErstelltAm { get; set; }
        public DateTime LetztesLogin { get; set; }
    }
}
