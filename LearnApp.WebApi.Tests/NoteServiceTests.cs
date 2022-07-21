using Bogus;
using FakeItEasy;
using LearnApp.BLL.Models.Request;
using LearnApp.BLL.Services;
using LearnApp.DAL.Context;
using LearnApp.DAL.DataInitializer;
using LearnApp.Helper.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LearnApp.WebApi.Tests
{
    public class NoteServiceTests
    {
        [Fact]
        public async void CreateNoteIfSomePropsAreIncorent()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                //.WithMock<AccountService>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            //var accountService = app.Services.GetRequiredService<AccountService>();
            //A.CallTo(() => accountService.LoginAsync(A<RequestLoginModel>.Ignored))
            //    .Returns((context.User.First(), string.Empty));

            var user = context.User.FirstOrDefault();
            Assert.NotNull(user);

            var service = app.Services.GetRequiredService<NoteService>();
            var result = await service.CreateNoteAsync(new RequestNoteModel
            {
                Title = faker.Name.JobTitle(),
                Description = faker.Lorem.Text(),
                Link = faker.Internet.Url(),
                IsVisible = faker.Random.Bool(),
                NoteTypeCode = context.NoteType.First().Code,
                UserGuid = user!.Guid
            });

            Assert.NotNull(result);
        }

        [Fact]
        public async void UpdateNoteIfNoteIsExist()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            var existNote = context.Note.FirstOrDefault();
            string expectedTitle = existNote!.Title;
            Assert.NotNull(existNote);
            Assert.NotEmpty(expectedTitle);

            var service = app.Services.GetRequiredService<NoteService>();
            var result = await service.UpdateNoteAsync(existNote.Guid, new RequestNoteModel
            {
                Title = faker.Lorem.Word(),
                Description = faker.Lorem.Text(),
                Link = existNote.Link,
                IsVisible = existNote.IsVisible,
                NoteTypeCode = existNote.NoteTypeCode,
                UserGuid = existNote.UserGuid
            });

            var updatedNote = context.Note.First(n => n.Guid == existNote.Guid);

            Assert.Empty(result);
            Assert.NotEqual(expectedTitle, updatedNote.Title);
        }

        [Fact]
        public async void RemoveNoteIfNoteIsExist()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .WithMock<AccountService>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            var note = context.Note.FirstOrDefault();
            Assert.NotNull(note);

            var service = app.Services.GetRequiredService<NoteService>();
            string result = await service.RemoveNoteAsync(new RequestRemoveDataModel
            {
                Guid = note!.Guid,
                UserGuid = note.UserGuid
            });

            var removedNote = context.Note.FirstOrDefault(n => n.Guid == note.Guid);
            Assert.Null(removedNote);
            Assert.Empty(result);
        }

        [Fact]
        public async void RemoveNoteIfNoteIsNotExistOrUserIsNotCreator()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .WithMock<AccountService>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            var note = context.Note.FirstOrDefault();
            Assert.NotNull(note);

            var service = app.Services.GetRequiredService<NoteService>();
            var resultNotExistGuid = await service.RemoveNoteAsync(new RequestRemoveDataModel
            {
                Guid = Guid.NewGuid(),
                UserGuid = note!.UserGuid
            });
            Assert.NotEmpty(resultNotExistGuid);

            var resultUserIsNotCreator = await service.RemoveNoteAsync(new RequestRemoveDataModel
            {
                Guid = note.Guid,
                UserGuid = Guid.NewGuid()
            });
            Assert.NotEmpty(resultUserIsNotCreator);

            var notRemovedNote = context.Note.FirstOrDefault(n => n.Guid == note.Guid);
            Assert.NotNull(notRemovedNote);
        }
    }
}
