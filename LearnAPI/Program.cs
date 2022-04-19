using LearnEF.Context;
using LearnEF.Repos;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LearnEF.DataInitializer;
using LearnEF.Entities.IdentityModel;
using LearnAPI.Validate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Конфигурирование контекстов для внедрения зависимостей

builder.Services.AddDbContextPool<LearnContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnConnection"),
        o => o.EnableRetryOnFailure()));

//Добавление UserRepo и RoleRepo в контейнер DI
builder.Services.AddScoped<ILearnRepo, LearnRepo>();
builder.Services.AddScoped<ISourceLoreRepo, SourceLoreRepo>();

#endregion

#region Добавление сервиса Identity

//Добавление собственных валидаторов для аутентификации
builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();
builder.Services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>(
    service => new CustomPasswordValidator(6));

//Параметры проверки пароля
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<LearnContext>()
    .AddDefaultTokenProviders();

#endregion

//Регистрация фильтра исключений и возвращение к стилю Pascal
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Использование аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//Получение области действия контейнера DI
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //Извлечение объекта LearnContext, UserManager и RoleManager из контейнера
    var context = services.GetRequiredService<LearnContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    //Вызов инициализатора данных
    Initializer.RecreateDatabase(context);
    await Initializer.InitializeData(context, userManager, roleManager);
}

app.Run();
