//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication3
{
    using System;
    using System.Collections.Generic;
    
    public partial class DuplicateSearch
    {
        public int HistoryID { get; set; }
        public string Directory { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public string UserID { get; set; }
        public Nullable<int> Size { get; set; }
        public int Id { get; set; }
    
        public virtual History History { get; set; }
    }
}
