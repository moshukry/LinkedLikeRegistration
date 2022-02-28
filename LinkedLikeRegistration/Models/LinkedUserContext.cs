using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace LinkedLikeRegistration.Models
{
    public partial class LinkedUserContext : DbContext
    {
        public LinkedUserContext()
            : base("name=LinkedUserContext")
        {
        }

        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<News> News { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(e => e.Skills)
                .WithMany(e => e.Users)
                .Map(m => m.ToTable("User_skills").MapLeftKey("user_id").MapRightKey("skill_id"));
        }
    }
}
