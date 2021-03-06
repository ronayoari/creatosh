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
    
    public partial class Game
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Game()
        {
            this.Activities = new HashSet<Activity>();
            this.Comments = new HashSet<Comment>();
            this.Measures_Values_In_Game = new HashSet<Measures_Values_In_Game>();
            this.Categories = new HashSet<Category>();
            this.Categories1 = new HashSet<Category>();
        }
    
        public int game_ID { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public System.DateTime publish_date { get; set; }
        public int creator_ID { get; set; }
        public int amount_of_likes { get; set; }
        public int grade_level { get; set; }
        public string template_name { get; set; }
        public string teacher_comment { get; set; }
        public string subject_name { get; set; }
        public Nullable<int> status { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Activity> Activities { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual Template Template { get; set; }
        public virtual Template Template1 { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Measures_Values_In_Game> Measures_Values_In_Game { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Category> Categories1 { get; set; }
    }
}
