using Bogus;
using LearnApp.BLL.Models.Request;
using LearnApp.BLL.Services;
using LearnApp.DAL.Context;
using LearnApp.DAL.DataInitializer;
using LearnApp.Helper.Tests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace LearnApp.WebApi.Tests
{
    public class AccountServiceTests
    {
        [Fact]
        public async void CreateAccountIfUserEntersUniqueEmail()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            string fakeEmail = faker.Internet.Email();
            Assert.NotEmpty(fakeEmail);

            var service = app.Services.GetRequiredService<AccountService>();
            await service.RegisterAsync(new RequestRegisterModel
            {
                Login = fakeEmail,
                Password = faker.Internet.Password(),
                Surname = faker.Name.LastName(),
                Name = faker.Name.FirstName(),
            });

            var user = context.User.LastOrDefault();
            Assert.NotNull(user);
            Assert.Equal(user!.Login, fakeEmail);
        }

        [Fact]
        public async void LoginToAccountIfUserIsAlreadyRegistered()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            var existUser = context.User.First();

            var service = app.Services.GetRequiredService<AccountService>();
            (var result, var error) = await service.LoginAsync(new RequestLoginModel
            {
                Login = existUser.Login,
                Password = "123"
            });

            Assert.NotNull(result);
            Assert.Empty(error);
        }

        [Fact]
        public async void FailLoginToAccountIfUserIsAlreadyRegistered()
        {
            var app = new SystemUnderTestBuilder<Program>()
                .WithInMemoryDb<LearnContext>()
                .Build();
            var faker = new Faker("ru");

            var context = app.Services.GetRequiredService<LearnContext>();
            await Initializer.FillDbWithTestData(context);

            var existUser = context.User.First();

            var service = app.Services.GetRequiredService<AccountService>();
            (var result, var error) = await service.LoginAsync(new RequestLoginModel
            {
                Login = existUser.Login,
                Password = "failed"
            });

            Assert.Null(result);
            Assert.NotEmpty(error);
        }
    }
}