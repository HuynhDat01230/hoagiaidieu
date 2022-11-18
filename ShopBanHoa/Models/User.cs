﻿//------------------------------------------------------------------------------
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
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;
    
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            this.Carts = new HashSet<Cart>();
            this.Transactions = new HashSet<Transaction>();
        }
    
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Yêu cầu tên người dùng")]
        [DisplayName("Tên người dùng")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Yêu cầu tên email")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Yêu cầu tên người dùng")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage ="Tối thiểu 8 ký tự, cả số và chữ")]
        public string Password { get; set; }

        [Required]
        [RegularExpression(@"/\(?([0-9]{3})\)?([ .-]?)([0-9]{3})\2([0-9]{4})/", ErrorMessage = "Nhập sai số điện thoại")]
        public string Phone { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Password không trùng")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        public int UserTypeID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cart> Carts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual UserType UserType { get; set; }
    }
}