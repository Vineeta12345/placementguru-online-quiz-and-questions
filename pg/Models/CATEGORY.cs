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
    
    public partial class CATEGORY
    {
        public CATEGORY()
        {
            this.QUESTIONS = new HashSet<QUESTION>();
        }
    
        public int cat_id { get; set; }
        public string cat_name { get; set; }
        public Nullable<int> cat_fk_adid { get; set; }
        public string cat_encrypt { get; set; }
    
        public virtual TBL_ADMIN TBL_ADMIN { get; set; }
        public virtual ICollection<QUESTION> QUESTIONS { get; set; }
    }
}
