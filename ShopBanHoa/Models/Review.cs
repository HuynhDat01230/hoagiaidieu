//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ShopBanHoa.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public int ReviewID { get; set; }
        public int ProductID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public Nullable<int> Rating { get; set; }
        public string Content { get; set; }
        public System.DateTime Date { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}