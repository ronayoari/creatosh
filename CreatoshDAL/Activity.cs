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
    
    public partial class Activity
    {
        public int game_ID { get; set; }
        public int user_ID_send { get; set; }
        public int user_ID_receive { get; set; }
        public string comment { get; set; }
        public int read { get; set; }
        public int response { get; set; }
    
        public virtual Game Game { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
