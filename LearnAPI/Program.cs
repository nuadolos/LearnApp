using LearnEF.Context;
using LearnEF.Repos;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LearnEF.DataInitializer;
using LearnEF.Entities.IdentityModel;
using LearnAPI.Validate;
using LearnHTTP.EmailService;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Конфигурирование контекстов для внедрения зависимостей

builder.Services.AddDbContextPool<LearnContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnConnection"),
        o => o.EnableRetryOnFailure()));

builder.Services.AddControllersWithViews();

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
        options.Lockout.MaxFailedAccessAttempts = 1;
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<LearnContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

#endregion

//Регистрация фильтра исключений и возвращение к стилю Pascal
builder.Services.AddMvc().AddJsonOptions(
    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

#region Добавление сервиса для отправки сообщений на эл. почту пользователя

builder.Services.AddSingleton(
    builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>());
builder.Services.AddScoped<IEmailSender, EmailSender>();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

#endregion

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
