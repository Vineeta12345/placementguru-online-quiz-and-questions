//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TBL_ADMIN
    {
        public TBL_ADMIN()
        {
            this.CATEGORies = new HashSet<CATEGORY>();
        }
    
        public int AD_ID { get; set; }
        public string AD_NAME { get; set; }
        public string AD_PASSWORD { get; set; }
    
        public virtual ICollection<CATEGORY> CATEGORies { get; set; }
    }
}