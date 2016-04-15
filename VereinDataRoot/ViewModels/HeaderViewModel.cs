namespace VereinDataRoot.ViewModels
{
    using System.Collections.Generic;
    using Models;

    public class HeaderViewModel
    {
        public string VereinName { get; set; }
        public string UserName { get; set; }
        public List<MandantenBenutzerNavigation> Navigation { get; set; }
    }
}