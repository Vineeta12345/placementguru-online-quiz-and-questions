﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace pg.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PLACEMENTGURUEntities : DbContext
    {
        public PLACEMENTGURUEntities()
            : base("name=PLACEMENTGURUEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<CATEGORY> CATEGORies { get; set; }
        public DbSet<EXAM> EXAMs { get; set; }
        public DbSet<QUESTION> QUESTIONS { get; set; }
        public DbSet<TBL_ADMIN> TBL_ADMIN { get; set; }
        public DbSet<TBL_USER> TBL_USER { get; set; }
        public DbSet<ADMIN> ADMINs { get; set; }
    }
}
