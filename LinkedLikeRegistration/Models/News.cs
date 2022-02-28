namespace LinkedLikeRegistration.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class News
    {
        [Key]
        public int news_id { get; set; }

        [StringLength(150)]
        public string title { get; set; }

        [StringLength(150)]
        public string bref { get; set; }

        public string describtion { get; set; }

        [StringLength(100)]
        public string image { get; set; }

        public DateTime? datetime { get; set; }

        public int? user_id { get; set; }

        public int? cat_id { get; set; }

        public virtual Catalog Catalog { get; set; }

        public virtual User User { get; set; }
    }
}
