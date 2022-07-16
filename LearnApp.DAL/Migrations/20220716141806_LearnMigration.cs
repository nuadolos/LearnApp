using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearnApp.DAL.Migrations
{
    public partial class LearnMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupRoles",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRoles", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "GroupTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "NoteTypes",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteTypes", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "varchar(450)", unicode: false, maxLength: 450, nullable: false),
                    Salt = table.Column<string>(type: "varchar(450)", unicode: false, maxLength: 450, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Middlename = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Code = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    CodeTimeBlock = table.Column<DateTime>(type: "datetime2(0)", nullable: true),
                    UserRoleCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleCode",
                        column: x => x.UserRoleCode,
                        principalTable: "UserRoles",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubscribeUserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TrackedUserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FollowDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "(getdate())"),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Followers_Users_SubscribeUserGuid",
                        column: x => x.SubscribeUserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_Followers_Users_TrackedUserGuid",
                        column: x => x.TrackedUserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InviteCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    AdminCode = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    CreateDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    GroupTypeCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Groups_GroupTypes_GroupTypeCode",
                        column: x => x.GroupTypeCode,
                        principalTable: "GroupTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Groups_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Link = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "(getdate())"),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    NoteTypeCode = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Notes_NoteTypes_NoteTypeCode",
                        column: x => x.NoteTypeCode,
                        principalTable: "NoteTypes",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notes_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupUsers",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupRoleCode = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUsers", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_GroupUsers_GroupRoles_GroupRoleCode",
                        column: x => x.GroupRoleCode,
                        principalTable: "GroupRoles",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_GroupUsers_Groups_GroupGuid",
                        column: x => x.GroupGuid,
                        principalTable: "Groups",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupUsers_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "Learns",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "(getdate())"),
                    Deadline = table.Column<DateTime>(type: "datetime2(0)", nullable: false),
                    GroupGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Learns", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Learns_Groups_GroupGuid",
                        column: x => x.GroupGuid,
                        principalTable: "Groups",
                        principalColumn: "Guid");
                    table.ForeignKey(
                        name: "FK_Learns_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "ShareNotes",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShareNotes", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_ShareNotes_Notes_NoteGuid",
                        column: x => x.NoteGuid,
                        principalTable: "Notes",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShareNotes_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "Attaches",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rating = table.Column<byte>(type: "tinyint", nullable: true),
                    AttachmentDate = table.Column<DateTime>(type: "datetime2(0)", nullable: false, defaultValueSql: "(getdate())"),
                    LearnGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attaches", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Attaches_Learns_LearnGuid",
                        column: x => x.LearnGuid,
                        principalTable: "Learns",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attaches_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid");
                });

            migrationBuilder.CreateTable(
                name: "LearnDocs",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LearnGuid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Timestamp = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearnDocs", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_LearnDocs_Learns_LearnGuid",
                        column: x => x.LearnGuid,
                        principalTable: "Learns",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attaches_Guid",
                table: "Attaches",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attaches_LearnGuid",
                table: "Attaches",
                column: "LearnGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Attaches_UserGuid",
                table: "Attaches",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_Guid",
                table: "Followers",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Followers_SubscribeUserGuid",
                table: "Followers",
                column: "SubscribeUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_TrackedUserGuid",
                table: "Followers",
                column: "TrackedUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_Code",
                table: "GroupRoles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_GroupTypeCode",
                table: "Groups",
                column: "GroupTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Guid",
                table: "Groups",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_InviteCode_AdminCode",
                table: "Groups",
                columns: new[] { "InviteCode", "AdminCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_UserGuid",
                table: "Groups",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GroupTypes_Code",
                table: "GroupTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupGuid",
                table: "GroupUsers",
                column: "GroupGuid");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_GroupRoleCode",
                table: "GroupUsers",
                column: "GroupRoleCode");

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_Guid",
                table: "GroupUsers",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupUsers_UserGuid",
                table: "GroupUsers",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_LearnDocs_Guid",
                table: "LearnDocs",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LearnDocs_LearnGuid",
                table: "LearnDocs",
                column: "LearnGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Learns_GroupGuid",
                table: "Learns",
                column: "GroupGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Learns_Guid",
                table: "Learns",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Learns_UserGuid",
                table: "Learns",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_Guid",
                table: "Notes",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_NoteTypeCode",
                table: "Notes",
                column: "NoteTypeCode");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_UserGuid",
                table: "Notes",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTypes_Code",
                table: "NoteTypes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareNotes_Guid",
                table: "ShareNotes",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShareNotes_NoteGuid",
                table: "ShareNotes",
                column: "NoteGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ShareNotes_UserGuid",
                table: "ShareNotes",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_Code",
                table: "UserRoles",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Guid",
                table: "Users",
                column: "Guid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Login_Salt",
                table: "Users",
                columns: new[] { "Login", "Salt" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleCode",
                table: "Users",
                column: "UserRoleCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attaches");

            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DropTable(
                name: "GroupUsers");

            migrationBuilder.DropTable(
                name: "LearnDocs");

            migrationBuilder.DropTable(
                name: "ShareNotes");

            migrationBuilder.DropTable(
                name: "GroupRoles");

            migrationBuilder.DropTable(
                name: "Learns");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "NoteTypes");

            migrationBuilder.DropTable(
                name: "GroupTypes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");
        }
    }
}
