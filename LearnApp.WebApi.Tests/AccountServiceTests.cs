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
                Middlename = string.Empty
            });

            var user = context.User.LastOrDefault();
            Assert.NotNull(user);
            Assert.Equal(user!.Login, fakeEmail);
        }
    }
}