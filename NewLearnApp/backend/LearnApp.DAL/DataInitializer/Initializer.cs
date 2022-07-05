using LearnApp.DAL.Context;
using LearnApp.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.DAL.DataInitializer
{
    /// <summary>
    /// Удаляет, восстанавливает и заполняет начальными данными БД
    /// </summary>
    static public class Initializer
    {
        public static async Task InitializeData(LearnContext context)
        {
            List<User> users = new List<User>()
            {
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    Login = "2nuadolos1@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Иванов",
                    Name = "Владимир"
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    Login = "tester@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Иванов",
                    Name = "Андрей"
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    Login = "zxc@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Решимова",
                    Name = "Анна"
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    Login = "qwerty@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Бананова",
                    Name = "Виктория"
                },
                new User() {
                    Id = Guid.NewGuid().ToString(),
                    Login = "wasd@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Борисов",
                    Name = "Николай"
                }
            };

            users.ForEach(u => context.User.Add(u));

            List<SourceLore> lores = new List<SourceLore>()
            {
                new SourceLore() { Name = "Книга"},
                new SourceLore() { Name = "Уроки"}
            };

            lores.ForEach(lore => context.SourceLore.Add(lore));
            context.SaveChanges();

            List<GroupType> groupTypes = new List<GroupType>()
            {
                new GroupType { Name = "Равноправный" },
                new GroupType { Name = "Класс" }
            };

            groupTypes.ForEach(gropType => context.GroupType.Add(gropType));

            List<GroupRole> groupRoles = new List<GroupRole>()
            {
                new GroupRole { Name = "Студент" },
                new GroupRole { Name = "Преподаватель" },
                new GroupRole { Name = "Общий" }
            };

            groupRoles.ForEach(gropRole => context.GroupRole.Add(gropRole));
           
            await context.SaveChangesAsync();

            List<Note> notes = new List<Note>
            {
                new Note {
                    Title = "Программирование на равне",
                    Link = "https://vk.com/audios195148235",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    SourceLoreId = 1,
                    UserId = users[0].Id
                },

                new Note {
                    Title = "Точка соприкосновения",
                    Link = "https://github.com/nuadolos",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    SourceLoreId = 1,
                    UserId = users[2].Id
                },

                new Note {
                    Title = "Дерево молниеносное",
                    Link = "https://vk.com/audios195148235",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    SourceLoreId = 1,
                    UserId = users[0].Id
                },
            };

            notes.ForEach(note => context.Note.Add(note));

            List<Group> groups = new List<Group>()
            {
                new Group {
                    Name = "TestGroup1",
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    GroupTypeId = 1,
                    UserId = users[3].Id
                },

                new Group {
                    Name = "TestGroup2",
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    GroupTypeId = 2,
                    UserId = users[0].Id
                },

                new Group {
                    Name = "TestGroup3",
                    CodeAdmin = Guid.NewGuid().ToString(),
                    CodeInvite = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    GroupTypeId = 1,
                    UserId = users[2].Id
                }
            };

            context.Group.AddRange(groups);

            await context.SaveChangesAsync();

            List<Learn> learns = new List<Learn>()
            {
                new Learn()
                {
                    Title = "test1",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    UserId = users[0].Id,
                    GroupId = 1
                },

                new Learn()
                {
                    Title = "test2",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    UserId = users[0].Id,
                    GroupId = 2
                },

                new Learn()
                {
                    Title = "TEST3",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    UserId = users[0].Id,
                    GroupId = 1
                },

                new Learn()
                {
                    Title = "TEST4",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    UserId = users[2].Id,
                    GroupId = 2
                },

                new Learn()
                {
                    Title = "TEST5",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    UserId = users[1].Id,
                    GroupId = 1
                }
            };

            learns.ForEach(learn => context.Learn.Add(learn));

            List<GroupUser> groupUsers = new List<GroupUser>()
            {
                new GroupUser { GroupId = 1, UserId = users[0].Id, GroupRoleId = 2 },
                new GroupUser { GroupId = 1, UserId = users[3].Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 1, UserId = users[4].Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 2, UserId = users[1].Id, GroupRoleId = 3 },
                new GroupUser { GroupId = 2, UserId = users[2].Id, GroupRoleId = 3 }
            };

            groupUsers.ForEach(groupUser => context.GroupUser.Add(groupUser));

            List<ShareNote> shareNotes = new List<ShareNote>()
            {
                new ShareNote { NoteId = 1, UserId = users[1].Id, CanChange = true },
                new ShareNote { NoteId = 2, UserId = users[3].Id, CanChange = true },
                new ShareNote { NoteId = 3, UserId = users[4].Id, CanChange = false }
            };

            shareNotes.ForEach(shareLearn => context.ShareNote.Add(shareLearn));

            List<Follow> friends = new List<Follow>()
            {
                new Follow { SubscribeUserId = users[0].Id, TrackedUserId = users[2].Id, FollowDate = DateTime.Now },
                new Follow { SubscribeUserId = users[1].Id, TrackedUserId = users[3].Id, FollowDate = DateTime.Now },
                new Follow { SubscribeUserId = users[0].Id, TrackedUserId = users[4].Id, FollowDate = DateTime.Now }
            };

            friends.ForEach(fr => context.Follow.Add(fr));

            context.SaveChanges();

            #region test

            //using (Stream txtFile = new FileStream(@"C:\Users\nuadolos\Desktop\Bro.txt", FileMode.Open))
            //{
            //    using (StreamReader data = new StreamReader(txtFile))
            //    {
            //        string? lines;

            //        while ((lines = data.ReadLine()) != null)
            //        {
            //            string[]? datas = lines.Split("\t");

            //            Learn learn = new Learn()
            //            {
            //                Title = datas[1],
            //                Link = datas[2],
            //                CreateDate = DateTime.Parse(datas[3]),
            //                DateReading = DateTime.Parse(datas[4]),
            //                Image = datas[5],
            //                SourceLoreId = int.Parse(datas[6]),
            //                IsStudying = bool.Parse(datas[7])
            //            };

            //            learns.Add(learn);
            //        }
            //    }
            //}

            #endregion
        }

        public static void RecreateDatabase(LearnContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }

        public static void ClearData(LearnContext context)
        {
            var executeDeleteSql = (LearnContext context, string tableName) =>
            {
                var rawSqlString = $"Delete from dbo.{tableName}";
                context.Database.ExecuteSqlRaw(rawSqlString);
            };

            var resetIdentity = (LearnContext context) =>
            {
                var tables = new[] { nameof(LearnApp), nameof(SourceLore) };

                foreach (var table in tables)
                {
                    var rawSqlString = $"DBCC CHECKIDENT (\"dbo.{table}\", RESEED, 0);";
                    context.Database.ExecuteSqlRaw(rawSqlString);
                }
            };

            executeDeleteSql(context, nameof(LearnApp));
            executeDeleteSql(context, nameof(SourceLore));

            resetIdentity(context);
        }
    }
}
