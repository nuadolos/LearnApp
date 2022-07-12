using LearnApp.DAL.Entities;
using LearnApp.DAL.Entities.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable disable

namespace LearnApp.DAL.Context
{
    public class LearnContext : DbContext
    {
        public LearnContext()
        { }

        public LearnContext(DbContextOptions options) : base(options)
        { }

        public DbSet<User> User { get; set; }
        public DbSet<NoteType> NoteType { get; set; }
        public DbSet<Note> Note { get; set; }
        public DbSet<ShareNote> ShareNote { get; set; }
        public DbSet<Learn> Learn { get; set; }
        public DbSet<Attach> Attaches { get; set; }
        public DbSet<LearnDoc> LearnDocuments { get; set; }
        public DbSet<Follower> Follow { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupUser> GroupUser { get; set; }
        public DbSet<GroupRole> GroupRole { get; set; }
        public DbSet<GroupType> GroupType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder = Entities.Attach.OnModelCreating(modelBuilder);
            modelBuilder = Entities.Follower.OnModelCreating(modelBuilder);
            modelBuilder = Entities.Group.OnModelCreating(modelBuilder);
            modelBuilder = Entities.Learn.OnModelCreating(modelBuilder);
            modelBuilder = Entities.Note.OnModelCreating(modelBuilder);

            #region ForeignKey OnDelete NoAction

            modelBuilder.Entity<Entities.Follower>()
                .HasOne(e => e.SubscribeUser)
                .WithMany(e => e.SubscribeUsers)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.Follower>()
                .HasOne(e => e.TrackedUser)
                .WithMany(e => e.TrackedUsers)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.Learn>()
                .HasOne(e => e.User)
                .WithMany(e => e.Learns)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.Attach>()
                .HasOne(e => e.User)
                .WithMany(e => e.Attaches)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.ShareNote>()
                .HasOne(e => e.User)
                .WithMany(e => e.ShareNotes)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.Learn>()
                .HasOne(e => e.Group)
                .WithMany(e => e.Learns)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Entities.GroupUser>()
                .HasOne(e => e.User)
                .WithMany(e => e.GroupUsers)
                .OnDelete(DeleteBehavior.NoAction);

            #endregion
        }
    }
}
