//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CreatoshDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Metrics_Rating
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Metrics_Rating()
        {
            this.Measures_Values_In_Game = new HashSet<Measures_Values_In_Game>();
        }
    
        public string subject_name { get; set; }
        public string measure { get; set; }
        public int measrue_percent { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Measures_Values_In_Game> Measures_Values_In_Game { get; set; }
        public virtual Subject Subject { get; set; }
    }
}