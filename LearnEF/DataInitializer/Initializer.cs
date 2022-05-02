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
            if (await roleManager.FindByNameAsync("tester") == null)
                await roleManager.CreateAsync(new IdentityRole("tester"));

            if (await roleManager.FindByNameAsync("admin") == null)
                await roleManager.CreateAsync(new IdentityRole("admin"));

            if (await roleManager.FindByNameAsync("student") == null)
                await roleManager.CreateAsync(new IdentityRole("student"));

            if (await roleManager.FindByNameAsync("teacher") == null)
                await roleManager.CreateAsync(new IdentityRole("teacher"));

            if (await userManager.FindByNameAsync("tester") == null)
            {
                User u = new User { 
                    Email = "tester@nuadolos.com", UserName = "tester@nuadolos.com",
                    Surname = "Иванов", Name = "Андрей"
                };

                IdentityResult result = await userManager.CreateAsync(u, "tester1");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, "tester");
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

            List<SourceLore> lores = new List<SourceLore>()
            {
                new SourceLore() { Name = "Книга"},
                new SourceLore() { Name = "Уроки"}
            };

            lores.ForEach(lore => context.SourceLore.Add(lore));
            context.SaveChanges();

            User newUser = await userManager.FindByNameAsync("2nuadolos1@gmail.com");

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
                    UserId = newUser.Id,
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
                    UserId = newUser.Id,
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
                    UserId = newUser.Id,
                    IsStudying = false
                },
            };

            learns.ForEach(learn => context.Learn.Add(learn));
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
