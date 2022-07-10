using Microsoft.AspNetCore.Http.Features;
using LearnApp.DAL.Context;
using LearnApp.DAL.DataInitializer;
using LearnApp.Helper.EmailService;
using LearnApp.DAL;
using LearnApp.WebApi.JWT;
using LearnApp.BL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDALService(builder.Configuration);
builder.Services.AddBLService();

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

    //Извлечение объекта LearnContext из контейнера
    var context = services.GetRequiredService<LearnContext>();

    //Вызов инициализатора данных
    Initializer.RecreateDatabase(context);
    await Initializer.InitializeData(context);
}

app.Run();
