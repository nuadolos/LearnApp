using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using LearnApp.DAL.Context;
using LearnApp.DAL.DataInitializer;
using LearnApp.DAL.Repos;
using LearnApp.WebApi.Helper;
using LearnApp.Helper.EmailService;

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
