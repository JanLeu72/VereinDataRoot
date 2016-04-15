using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class VereinAdresse
    {
        public int VereinAdresseId { get; set; }
        public int MandantId { get; set; }
        public int MandantAdressTypeId { get; set; }
        public bool Standart { get; set; }
        public string Anrede { get; set; }
        public string Name { get; set; }
        public string Vorname { get; set; }
        public string Strasse { get; set; }
        public string Plz { get; set; }
        public string Ort { get; set; }
        public string Postfach { get; set; }
        public string AdresseZusatz { get; set; }
        public string NameZusatz { get; set; }
        public string Mail { get; set; }
        public string FormMessage { get; set; }
        public string MandantName { get; set; }
        public List<KeyValueModel> Anreden { get; set; } 
    }
}
