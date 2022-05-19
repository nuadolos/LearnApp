using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.Context
{
    public class LearnContext : IdentityDbContext<User>
    {
        internal LearnContext()
        { }

        public LearnContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Entities.SourceLore> SourceLore { get; set; }
        public DbSet<Entities.Note> Note { get; set; }
        public DbSet<Entities.ShareNote> ShareNote { get; set; }
        public DbSet<Entities.Learn> Learn { get; set; }
        public DbSet<Entities.LearnDocuments> LearnDocuments { get; set; }
        public DbSet<Entities.Friend> Friend { get; set; }
        public DbSet<Entities.Group> Group { get; set; }
        public DbSet<Entities.GroupUser> GroupUser { get; set; }
        public DbSet<Entities.GroupRole> GroupRole { get; set; }
        public DbSet<Entities.GroupType> GroupType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = @"server=.\sqlexpress;database=KnowledgeLibrary;
                    integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

                optionsBuilder.UseSqlServer(connectionString,
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Entities.Attach>()
                .HasOne(e => e.Learn)
                .WithMany(e => e.Attach)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Friend>()
                .HasOne(e => e.SentUser)
                .WithMany(e => e.SentUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Friend>()
                .HasOne(e => e.SentUser)
                .WithMany(e => e.SentUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Friend>()
                .HasOne(e => e.AcceptedUser)
                .WithMany(e => e.AcceptedUser)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.Note>()
                .HasOne(e => e.SourceLore)
                .WithMany(e => e.Note)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.ShareNote>()
                .HasOne(e => e.Note)
                .WithMany(e => e.ShareNote)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Group>()
                .HasOne(e => e.GroupType)
                .WithMany(e => e.Group)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Group>()
                .HasOne(e => e.User)
                .WithMany(e => e.Group)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.GroupUser>()
                .HasOne(e => e.User)
                .WithMany(e => e.GroupUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.GroupUser>()
                .HasOne(e => e.Group)
                .WithMany(e => e.GroupUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.GroupUser>()
                .HasOne(e => e.GroupRole)
                .WithMany(e => e.GroupUser)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
