using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.Context
{
    public class LearnContext : DbContext
    {
        internal LearnContext()
        { }

        public LearnContext(DbContextOptions options) : base(options)
        { }

        public DbSet<Entities.User> User { get; set; }
        public DbSet<Entities.SourceLore> SourceLore { get; set; }
        public DbSet<Entities.Note> Note { get; set; }
        public DbSet<Entities.ShareNote> ShareNote { get; set; }
        public DbSet<Entities.Learn> Learn { get; set; }
        public DbSet<Entities.Attach> Attaches { get; set; }
        public DbSet<Entities.LearnDocuments> LearnDocuments { get; set; }
        public DbSet<Entities.Follow> Follow { get; set; }
        public DbSet<Entities.Group> Group { get; set; }
        public DbSet<Entities.GroupUser> GroupUser { get; set; }
        public DbSet<Entities.GroupRole> GroupRole { get; set; }
        public DbSet<Entities.GroupType> GroupType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = @"server=.\sqlexpress;database=KnowledgeLibraryTest;
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

            modelBuilder.Entity<Entities.Follow>()
                .HasOne(e => e.SubscribeUser)
                .WithMany(e => e.SubscribeUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Follow>()
                .HasOne(e => e.SubscribeUser)
                .WithMany(e => e.SubscribeUser)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Entities.Follow>()
                .HasOne(e => e.TrackedUser)
                .WithMany(e => e.TrackedUser)
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
