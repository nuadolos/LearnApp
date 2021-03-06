// <auto-generated />
using System;
using LearnApp.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LearnApp.DAL.Migrations
{
    [DbContext(typeof(LearnContext))]
    partial class LearnContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("LearnApp.DAL.Entities.Attach", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("AttachmentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LearnGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte?>("Rating")
                        .HasColumnType("tinyint");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("LearnGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("Attaches");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Follower", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("FollowDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<Guid>("SubscribeUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("TrackedUserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("SubscribeUserGuid");

                    b.HasIndex("TrackedUserGuid");

                    b.ToTable("Followers");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Group", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AdminCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GroupTypeCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<Guid>("InviteCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier")
                        .HasDefaultValueSql("(newid())");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("GroupTypeCode");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("UserGuid");

                    b.HasIndex("InviteCode", "AdminCode")
                        .IsUnique();

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupRole", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("GroupRoles");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupType", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("GroupTypes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupUser", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("GroupRoleCode")
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("GroupGuid");

                    b.HasIndex("GroupRoleCode");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("UserGuid");

                    b.ToTable("GroupUsers");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Learn", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<DateTime>("Deadline")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("GroupGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("GroupGuid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("UserGuid");

                    b.ToTable("Learns");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.LearnDoc", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("FilePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("LearnGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("LearnGuid");

                    b.ToTable("LearnDocs");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Note", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreateDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2(0)")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("bit");

                    b.Property<string>("Link")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("NoteTypeCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("NoteTypeCode");

                    b.HasIndex("UserGuid");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.NoteType", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("NoteTypes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.ShareNote", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("NoteGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<Guid>("UserGuid")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("NoteGuid");

                    b.HasIndex("UserGuid");

                    b.ToTable("ShareNotes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.User", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasMaxLength(6)
                        .IsUnicode(false)
                        .HasColumnType("varchar(6)");

                    b.Property<DateTime?>("CodeTimeBlock")
                        .HasColumnType("datetime2(0)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Middlename")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(450)
                        .IsUnicode(false)
                        .HasColumnType("varchar(450)");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<byte[]>("Timestamp")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("UserRoleCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Guid");

                    b.HasIndex("Guid")
                        .IsUnique();

                    b.HasIndex("UserRoleCode");

                    b.HasIndex("Login", "Salt")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.UserRole", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Code");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Attach", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.Learn", "Learn")
                        .WithMany("Attaches")
                        .HasForeignKey("LearnGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("Attaches")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Learn");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Follower", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.User", "SubscribeUser")
                        .WithMany("SubscribeUsers")
                        .HasForeignKey("SubscribeUserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "TrackedUser")
                        .WithMany("TrackedUsers")
                        .HasForeignKey("TrackedUserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("SubscribeUser");

                    b.Navigation("TrackedUser");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Group", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.GroupType", "GroupType")
                        .WithMany("Groups")
                        .HasForeignKey("GroupTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("Groups")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupUser", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.Group", "Group")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.GroupRole", "GroupRole")
                        .WithMany("GroupUsers")
                        .HasForeignKey("GroupRoleCode");

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("GroupUsers")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("GroupRole");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Learn", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.Group", "Group")
                        .WithMany("Learns")
                        .HasForeignKey("GroupGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("Learns")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.LearnDoc", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.Learn", "Learn")
                        .WithMany("LearnDocs")
                        .HasForeignKey("LearnGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Learn");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Note", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.NoteType", "NoteType")
                        .WithMany("Notes")
                        .HasForeignKey("NoteTypeCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("Notes")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NoteType");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.ShareNote", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.Note", "Note")
                        .WithMany("ShareNotes")
                        .HasForeignKey("NoteGuid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LearnApp.DAL.Entities.User", "User")
                        .WithMany("ShareNotes")
                        .HasForeignKey("UserGuid")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Note");

                    b.Navigation("User");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.User", b =>
                {
                    b.HasOne("LearnApp.DAL.Entities.UserRole", "UserRole")
                        .WithMany("Users")
                        .HasForeignKey("UserRoleCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserRole");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Group", b =>
                {
                    b.Navigation("GroupUsers");

                    b.Navigation("Learns");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupRole", b =>
                {
                    b.Navigation("GroupUsers");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.GroupType", b =>
                {
                    b.Navigation("Groups");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Learn", b =>
                {
                    b.Navigation("Attaches");

                    b.Navigation("LearnDocs");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.Note", b =>
                {
                    b.Navigation("ShareNotes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.NoteType", b =>
                {
                    b.Navigation("Notes");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.User", b =>
                {
                    b.Navigation("Attaches");

                    b.Navigation("GroupUsers");

                    b.Navigation("Groups");

                    b.Navigation("Learns");

                    b.Navigation("Notes");

                    b.Navigation("ShareNotes");

                    b.Navigation("SubscribeUsers");

                    b.Navigation("TrackedUsers");
                });

            modelBuilder.Entity("LearnApp.DAL.Entities.UserRole", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
