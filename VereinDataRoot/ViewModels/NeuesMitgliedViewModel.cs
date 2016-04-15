using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Models;

namespace VereinDataRoot.ViewModels
{
    public class NeuesMitgliedViewModel : MitgliedModel
    {
        public List<KeyValueModel> Anreden { get; set; }
        public List<KeyValueModel> MitgliedschaftTypen { get; set; }
        public List<KeyValueModel> MandantenGruppen { get; set; }
        public List<KeyValueModel> Laender { get; set; } 
        public string FormMessage { get; set; }
    }
}