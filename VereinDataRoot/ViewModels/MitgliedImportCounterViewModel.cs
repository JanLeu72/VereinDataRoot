namespace VereinDataRoot.ViewModels
{
    using System.Collections.Generic;

    public class MitgliedImportCounterViewModel
    {
        public List<MitgliedImportErrorViewModel> MitgliederFehler { get; set; }
        public int TotalImport { get; set; }
        public int TotalFehler { get; set; }
    }
}