//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Mandant
    {
        public Mandant()
        {
            this.Mitglieders = new HashSet<Mitglied>();
            this.MandantenBenutzerGruppens = new HashSet<MandantenBenutzerGruppen>();
        }
    
        public int MandantId { get; set; }
        public string MandantName { get; set; }
        public System.DateTime ErstelltAm { get; set; }
        public bool Aktive { get; set; }
        public int AnzahlMitgliederMax { get; set; }
    
        public virtual ICollection<Mitglied> Mitglieders { get; set; }
        public virtual ICollection<MandantenBenutzerGruppen> MandantenBenutzerGruppens { get; set; }
    }
}