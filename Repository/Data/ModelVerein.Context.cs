﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class VereinDBEntities : DbContext
    {
        public VereinDBEntities()
            : base("name=VereinDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Anrede> Anredes { get; set; }
        public virtual DbSet<Mandant> Mandants { get; set; }
        public virtual DbSet<Mitglied> Mitglieds { get; set; }
        public virtual DbSet<MitgliedschaftType> MitgliedschaftTypes { get; set; }
        public virtual DbSet<MandantenBenutzerGruppen> MandantenBenutzerGruppens { get; set; }
        public virtual DbSet<Laender> Laenders { get; set; }
        public virtual DbSet<RessortMandantMitglieder> RessortMandantMitglieders { get; set; }
        public virtual DbSet<Ressort> Ressorts { get; set; }
    }
}