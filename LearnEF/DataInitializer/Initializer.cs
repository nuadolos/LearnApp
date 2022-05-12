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
                    Email = "tester@nuadolos.com", UserName = "tester@mail.ru",
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
            User newUser2 = await userManager.FindByNameAsync("tester@mail.ru");
            User newUser3 = await userManager.FindByNameAsync("qwerty@gmail.com");
            User newUser4 = await userManager.FindByNameAsync("wasd@gmail.com");
            User newUser5 = await userManager.FindByNameAsync("zxc@gmail.com");

            List<Learn> learns = new List<Learn>()
            {
                new Learn()
                {
                    Title = "test1",
                    Link = "test11",
                    CreateDate = DateTime.Now,
                    DateReading = DateTime.Now,
                    Image = "test111",
                    SourceLoreId = 1,
                    UserId = newUser1.Id,
                    IsStudying = false
                },

                new Learn()
                {
                    Title = "test2",
                    Link = "test22",
                    CreateDate = DateTime.Now,
                    DateReading = DateTime.Now,
                    Image = "test222",
                    SourceLoreId = 2,
                    UserId = newUser2.Id,
                    IsStudying = true
                },

                new Learn()
                {
                    Title = "TEST3",
                    Link = "test33",
                    CreateDate = DateTime.Now,
                    DateReading = DateTime.Now,
                    Image = "test333",
                    SourceLoreId = 2,
                    UserId = newUser1.Id,
                    IsStudying = false
                },

                new Learn()
                {
                    Title = "TEST4",
                    Link = "test44",
                    CreateDate = DateTime.Now,
                    DateReading = DateTime.Now,
                    Image = "test444",
                    SourceLoreId = 1,
                    UserId = newUser3.Id,
                    IsStudying = false
                },

                new Learn()
                {
                    Title = "TEST5",
                    Link = "test55",
                    CreateDate = DateTime.Now,
                    DateReading = DateTime.Now,
                    Image = "test555",
                    SourceLoreId = 2,
                    UserId = newUser5.Id,
                    IsStudying = false
                }
            };

            learns.ForEach(learn => context.Learn.Add(learn));

            List<GroupType> groupTypes = new List<GroupType>()
            { 
                new GroupType { Name = "Равноправный" },
                new GroupType { Name = "Класс" }
            };

            groupTypes.ForEach(gropType => context.GroupType.Add(gropType));

            List<GroupRole> groupRoles = new List<GroupRole>()
            {
                new GroupRole { Name = "Студент" },
                new GroupRole { Name = "Учитель" },
                new GroupRole { Name = "Общий" }
            };

            groupRoles.ForEach(gropRole => context.GroupRole.Add(gropRole));
            context.SaveChanges();

            List<Group> groups = new List<Group>()
            {
                new Group {
                    Name = "TestGroup1",
                    Code = "563255",
                    CreateDate = DateTime.Now,
                    GroupTypeId = 1,
                    UserId = newUser4.Id
                },

                new Group {
                    Name = "TestGroup2",
                    Code = "781251",
                    CreateDate = DateTime.Now,
                    GroupTypeId = 2,
                    UserId = newUser1.Id
                }
            };

            context.Group.AddRange(groups);

            List<GroupLearn> groupLearns = new List<GroupLearn>()
            {
                new GroupLearn { GroupId = 1, LearnId = 1 },
                new GroupLearn { GroupId = 1, LearnId = 3 },
                new GroupLearn { GroupId = 1, LearnId = 5 },
                new GroupLearn { GroupId = 2, LearnId = 2 },
                new GroupLearn { GroupId = 2, LearnId = 4 }
            };

            groupLearns.ForEach(groupLearn => context.GroupLearn.Add(groupLearn));

            List<GroupUser> groupUsers = new List<GroupUser>()
            {
                new GroupUser { GroupId = 1, UserId = newUser1.Id, GroupRoleId = 2 },
                new GroupUser { GroupId = 1, UserId = newUser4.Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 1, UserId = newUser5.Id, GroupRoleId = 1 },
                new GroupUser { GroupId = 2, UserId = newUser2.Id, GroupRoleId = 3 },
                new GroupUser { GroupId = 2, UserId = newUser3.Id, GroupRoleId = 3 }
            };

            groupUsers.ForEach(groupUser => context.GroupUser.Add(groupUser));

            List<ShareLearn> shareLearns = new List<ShareLearn>()
            { 
                new ShareLearn { LearnId = 4, UserId = newUser1.Id, CanChange = true },
                new ShareLearn { LearnId = 5, UserId = newUser1.Id, CanChange = true },
                new ShareLearn { LearnId = 1, UserId = newUser3.Id, CanChange = false }
            };

            shareLearns.ForEach(shareLearn => context.ShareLearn.Add(shareLearn));

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
