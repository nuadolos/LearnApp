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
            List<UserRole> roles = new List<UserRole> 
            {
                new UserRole {
                    Code = "ADMIN"
                },
                new UserRole {
                    Code = "COMMON"
                }
            };

            roles.ForEach(role => context.UserRole.Add(role));

            List<NoteType> noteTypes = new List<NoteType>()
            {
                new NoteType() { Code = "BOOK"},
                new NoteType() { Code = "LESSONS"}
            };

            noteTypes.ForEach(noteType => context.NoteType.Add(noteType));

            List<GroupType> groupTypes = new List<GroupType>()
            {
                new GroupType { Code = "EQUAL" },
                new GroupType { Code = "CLASS" }
            };

            groupTypes.ForEach(gropType => context.GroupType.Add(gropType));

            List<GroupRole> groupRoles = new List<GroupRole>()
            {
                new GroupRole { Code = "STUDENT" },
                new GroupRole { Code = "TEACHER" },
                new GroupRole { Code = "GENERAL" }
            };

            groupRoles.ForEach(gropRole => context.GroupRole.Add(gropRole));

            await context.SaveChangesAsync();

            List<User> users = new List<User>()
            {
                new User() {
                    Login = "2nuadolos1@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Иванов",
                    Name = "Владимир",
                    Middlename = "Владимирович",
                    UserRoleCode = roles[0].Code
                },
                new User() {
                    Login = "tester@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Иванов",
                    Name = "Андрей",
                    Middlename = "Владимирович",
                    UserRoleCode = roles[1].Code
                },
                new User() {
                    Login = "zxc@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Решимова",
                    Name = "Анна",
                    Middlename = "Владимировна",
                    UserRoleCode = roles[0].Code
                },
                new User() {
                    Login = "qwerty@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Бананова",
                    Name = "Виктория",
                    Middlename = "Владимировна",
                    UserRoleCode = roles[1].Code
                },
                new User() {
                    Login = "wasd@gmail.com",
                    PasswordHash = "gNJLVahSBiE0Oh7Svxm89qSMINHD9z1I/UI92iziKYA=",
                    Salt = "fI+87R5gigoxRVVwwfdsVA==",
                    Surname = "Борисов",
                    Name = "Николай",
                    Middlename = "Владимирович",
                    UserRoleCode = roles[1].Code
                }
            };

            users.ForEach(u => context.User.Add(u));

            await context.SaveChangesAsync();

            List<Note> notes = new List<Note>
            {
                new Note {
                    Title = "Программирование на равне",
                    Link = "https://vk.com/audios195148235",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    NoteType = noteTypes[1],
                    User = users[0]
                },

                new Note {
                    Title = "Точка соприкосновения",
                    Link = "https://github.com/nuadolos",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    NoteType = noteTypes[0],
                    User = users[2]
                },

                new Note {
                    Title = "Дерево молниеносное",
                    Link = "https://vk.com/audios195148235",
                    CreateDate = DateTime.Now,
                    IsVisible = false,
                    NoteType = noteTypes[1],
                    User = users[0]
                },
            };

            notes.ForEach(note => context.Note.Add(note));

            List<Group> groups = new List<Group>()
            {
                new Group {
                    Title = "TestGroup1",
                    IsVisible = true,
                    GroupType = groupTypes[0],
                    User = users[3]
                },

                new Group {
                    Title = "TestGroup2",
                    IsVisible = true,
                    GroupType = groupTypes[1],
                    User = users[0]
                },

                new Group {
                    Title = "TestGroup3",
                    IsVisible = false,
                    GroupType = groupTypes[0],
                    User = users[2]
                }
            };

            await context.Group.AddRangeAsync(groups);

            await context.SaveChangesAsync();

            List<Learn> learns = new List<Learn>()
            {
                new Learn()
                {
                    Title = "test1",
                    Deadline = DateTime.Now.AddDays(7),
                    User = users[0],
                    Group = groups[0]
                },

                new Learn()
                {
                    Title = "test2",
                    Deadline = DateTime.Now.AddDays(7),
                    User = users[0],
                    Group = groups[1]
                },

                new Learn()
                {
                    Title = "TEST3",
                    Deadline = DateTime.Now.AddDays(7),
                    User = users[0],
                    Group = groups[0]
                },

                new Learn()
                {
                    Title = "TEST4",
                    Deadline = DateTime.Now.AddDays(7),
                    User = users[2],
                    Group = groups[1]
                },

                new Learn()
                {
                    Title = "TEST5",
                    Deadline = DateTime.Now.AddDays(7),
                    User = users[1],
                    Group = groups[0]
                }
            };

            learns.ForEach(learn => context.Learn.Add(learn));

            List<GroupUser> groupUsers = new List<GroupUser>()
            {
                new GroupUser { Group = groups[0], User = users[0], GroupRole = groupRoles[1] },
                new GroupUser { Group = groups[0], User = users[3], GroupRole = groupRoles[0] },
                new GroupUser { Group = groups[0], User = users[4], GroupRole = groupRoles[0] },
                new GroupUser { Group = groups[1], User = users[1], GroupRole = groupRoles[2] },
                new GroupUser { Group = groups[1], User = users[2], GroupRole = groupRoles[2] }
            };

            groupUsers.ForEach(groupUser => context.GroupUser.Add(groupUser));

            List<ShareNote> shareNotes = new List<ShareNote>()
            {
                new ShareNote { Note = notes[0], User = users[1] },
                new ShareNote { Note = notes[1], User = users[3] },
                new ShareNote { Note = notes[2], User = users[4] }
            };

            shareNotes.ForEach(shareLearn => context.ShareNote.Add(shareLearn));

            List<Follower> friends = new List<Follower>()
            {
                new Follower { SubscribeUser = users[0], TrackedUser = users[2] },
                new Follower { SubscribeUser = users[1], TrackedUser = users[3] },
                new Follower { SubscribeUser = users[0], TrackedUser = users[4] }
            };

            friends.ForEach(fr => context.Follower.Add(fr));

            context.SaveChanges();
        }

        public static void RecreateDatabase(LearnContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
