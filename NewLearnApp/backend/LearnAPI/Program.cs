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
using LearnWebAPI.Helper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

#region Конфигурирование контекстов для внедрения зависимостей

builder.Services.AddDbContextPool<LearnContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnConnection")));

builder.Services.AddScoped<ILearnRepo, LearnRepo>();
builder.Services.AddScoped<ISourceLoreRepo, SourceLoreRepo>();
builder.Services.AddScoped<IShareNoteRepo, ShareNoteRepo>();
builder.Services.AddScoped<IGroupRepo, GroupRepo>();
builder.Services.AddScoped<IFollowRepo, FollowRepo>();
builder.Services.AddScoped<INoteRepo, NoteRepo>();
builder.Services.AddScoped<IGroupUserRepo, GroupUserRepo>();
builder.Services.AddScoped<ILearnDocumentsRepo, LearnDocumentsRepo>();
builder.Services.AddScoped<IAttachRepo, AttachRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

#endregion

//Регистрация фильтра исключений и возвращение к стилю Pascal
//builder.Services.AddMvc().AddJsonOptions(
//    options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

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

builder.Services.AddCors();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Использование аутентификации и авторизации
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(options => options
    .WithOrigins(new[] { "http://localhost:3000" })
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
);

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

//Получение области действия контейнера DI
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //Извлечение объекта LearnContext, UserManager и RoleManager из контейнера
    var context = services.GetRequiredService<LearnContext>();

    //Вызов инициализатора данных
    Initializer.RecreateDatabase(context);
    await Initializer.InitializeData(context);
}

app.Run();
