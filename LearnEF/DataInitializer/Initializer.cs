using LearnEF.Context;
using LearnEF.Entities;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnEF.DataInitializer
{
    /// <summary>
    /// Удаляет, восстанавливает и заполняет начальными данными БД
    /// </summary>
    static public class Initializer
    {
        public static async Task InitializeData(LearnContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("common") == null)
                await roleManager.CreateAsync(new IdentityRole("common"));

            if (await userManager.FindByNameAsync("tester") == null)
            {
                User u = new User { 
                    Email = "tester@gmail.com", UserName = "tester@gmail.com",
                    Surname = "Иванов", Name = "Андрей"
                };

                IdentityResult result = await userManager.CreateAsync(u, "tester1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "common");
                }
            }

            if (await userManager.FindByNameAsync("2nuadolos1") == null)
            {
                User u = new User {
                    Email = "2nuadolos1@gmail.com", UserName = "2nuadolos1@gmail.com",
                    Surname = "Куракин", Name = "Владимир" 
                };

                IdentityResult result = await userManager.CreateAsync(u, "admin1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "admin");
                }
            }

            if (await userManager.FindByNameAsync("qwerty") == null)
            {
                User u = new User
                {
                    Email = "qwerty@gmail.com",
                    UserName = "qwerty@gmail.com",
                    Surname = "Бананова",
                    Name = "Виктория"
                };

                IdentityResult result = await userManager.CreateAsync(u, "vikvik");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "admin");
                }
            }

            if (await userManager.FindByNameAsync("wasd") == null)
            {
                User u = new User
                {
                    Email = "wasd@gmail.com",
                    UserName = "wasd@gmail.com",
                    Surname = "Борисов",
                    Name = "Николай"
                };

                IdentityResult result = await userManager.CreateAsync(u, "asd123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "common");
                }
            }

            if (await userManager.FindByNameAsync("zxc") == null)
            {
                User u = new User
                {
                    Email = "zxc@gmail.com",
                    UserName = "zxc@gmail.com",
                    Surname = "Решимова",
                    Name = "Анна"
                };

                IdentityResult result = await userManager.CreateAsync(u, "zxc123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "common");
                }
            }

            List<SourceLore> lores = new List<SourceLore>()
            {
                new SourceLore() { Name = "Книга"},
                new SourceLore() { Name = "Уроки"}
            };

            lores.ForEach(lore => context.SourceLore.Add(lore));
            context.SaveChanges();

            User newUser1 = await userManager.FindByNameAsync("2nuadolos1@gmail.com");
            User newUser2 = await userManager.FindByNameAsync("tester@gmail.com");
            User newUser3 = await userManager.FindByNameAsync("qwerty@gmail.com");
            User newUser4 = await userManager.FindByNameAsync("wasd@gmail.com");
            User newUser5 = await userManager.FindByNameAsync("zxc@gmail.com");

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
                    SourceLoreId = 1,
                    UserId = newUser1.Id
                },

                new Note {
                    Title = "Точка соприкосновения",
                    Link = "https://github.com/nuadolos",
                    CreateDate = DateTime.Now,
                    SourceLoreId = 1,
                    UserId = newUser3.Id
                },

                new Note {
                    Title = "Дерево молниеносное",
                    Link = "https://vk.com/audios195148235",
                    CreateDate = DateTime.Now,
                    SourceLoreId = 1,
                    UserId = newUser1.Id
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
                    UserId = newUser4.Id
                },

                new Group {
                    Name = "TestGroup2",
                    CreateDate = DateTime.Now,
                    IsVisible = true,
                    GroupTypeId = 2,
                    UserId = newUser1.Id
                },

                new Group {
                    Name = "TestGroup3",
                    CodeAdmin = Guid.NewGuid().ToString(),
                    CodeInvite = Guid.NewGuid().ToString(),
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    GroupTypeId = 1,
                    UserId = newUser3.Id
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
                    IsAttached = false,
                    UserId = newUser1.Id,
                    GroupId = 1
                },

                new Learn()
                {
                    Title = "test2",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    IsAttached = false,
                    UserId = newUser1.Id,
                    GroupId = 2
                },

                new Learn()
                {
                    Title = "TEST3",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    IsAttached = false,
                    UserId = newUser1.Id,
                    GroupId = 1
                },

                new Learn()
                {
                    Title = "TEST4",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    IsAttached = false,
                    UserId = newUser1.Id,
                    GroupId = 2
                },

                new Learn()
                {
                    Title = "TEST5",
                    CreateDate = DateTime.Now,
                    Deadline = DateTime.Now.AddDays(7),
                    IsAttached = false,
                    UserId = newUser1.Id,
                    GroupId = 1
                }
            };

            learns.ForEach(learn => context.Learn.Add(learn));

            List<GroupUser> groupUsers = new List<GroupUser>()
            {
                new GroupUser { GroupId = 1, UserId = newUser1.Id, GroupRoleId = 2 },
                new GroupUser { GroupId = 1, UserId = newUser4.Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 1, UserId = newUser5.Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 2, UserId = newUser2.Id, GroupRoleId = 3 },
                new GroupUser { GroupId = 2, UserId = newUser3.Id, GroupRoleId = 3 }
            };

            groupUsers.ForEach(groupUser => context.GroupUser.Add(groupUser));

            List<ShareNote> shareNotes = new List<ShareNote>()
            {
                new ShareNote { NoteId = 1, UserId = newUser2.Id, CanChange = true },
                new ShareNote { NoteId = 2, UserId = newUser4.Id, CanChange = true },
                new ShareNote { NoteId = 3, UserId = newUser5.Id, CanChange = false }
            };

            shareNotes.ForEach(shareLearn => context.ShareNote.Add(shareLearn));

            List<Friend> friends = new List<Friend>()
            {
                new Friend { SentUserId = newUser1.Id, AcceptedUserId = newUser3.Id, MakeFriend = DateTime.Now },
                new Friend { SentUserId = newUser2.Id, AcceptedUserId = newUser4.Id, MakeFriend = DateTime.Now },
                new Friend { SentUserId = newUser1.Id, AcceptedUserId = newUser5.Id, MakeFriend = DateTime.Now }
            };

            friends.ForEach(fr => context.Friend.Add(fr));

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
                var tables = new[] { nameof(Learn), nameof(SourceLore) };

                foreach (var table in tables)
                {
                    var rawSqlString = $"DBCC CHECKIDENT (\"dbo.{table}\", RESEED, 0);";
                    context.Database.ExecuteSqlRaw(rawSqlString);
                }
            };

            executeDeleteSql(context, nameof(Learn));
            executeDeleteSql(context, nameof(SourceLore));

            resetIdentity(context);
        }
    }
}
