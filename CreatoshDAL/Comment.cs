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
    
    public partial class Comment
    {
        public int comment_ID { get; set; }
        public int game_ID { get; set; }
        public int Auther_ID { get; set; }
        public int body { get; set; }
    
        public virtual User User { get; set; }
        public virtual Game Game { get; set; }
    }
}
