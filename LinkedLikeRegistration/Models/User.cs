namespace LinkedLikeRegistration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web.Mvc;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            News = new HashSet<News>();
            Skills = new HashSet<Skill>();
        }

        [Key]
        public int user_id { get; set; }

        [StringLength(50, ErrorMessage = "must be from 3 to 50 characters", MinimumLength = 3)]
        [Required(ErrorMessage = "*")]
        public string username { get; set; }


        //[Remote("checkemail", "registration", ErrorMessage = "Can't Use This Email!!")]
        [Required(ErrorMessage = "*")]
        [EmailAddress(ErrorMessage = "Invalid Email Address!")]
        [StringLength(320)]
        public string email { get; set; }


        [StringLength(50)]
        [Required(ErrorMessage = "*")]
        public string password { get; set; }


        [NotMapped]
        [Required(ErrorMessage = "*")]
        [System.ComponentModel.DataAnnotations.Compare("password", ErrorMessage = "password not match !!")]
        public string confirm_password { get; set; }


        //[Required(ErrorMessage = "*")]
        public int? age { get; set; }


        [StringLength(100)]
        public string address { get; set; }

        //[Required(ErrorMessage = "*")]
        [StringLength(150)]
        public string photo { get; set; }

        //[Required(ErrorMessage = "*")]
        [StringLength(150)]
        public string resume { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<News> News { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Skill> Skills { get; set; }
    }
}
