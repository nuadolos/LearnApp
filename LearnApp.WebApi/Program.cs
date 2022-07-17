using Microsoft.AspNetCore.Http.Features;
using LearnApp.DAL.Context;
using LearnApp.DAL.DataInitializer;
using LearnApp.Helper.EmailService;
using LearnApp.DAL;
using LearnApp.WebApi.JWT;
using LearnApp.BLL;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDALService(builder.Configuration);
builder.Services.AddBLService();

#region ���������� ������� ��� �������� ��������� �� ��. ����� ������������

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
builder.Services.AddSwaggerGen(options =>
{
    // ������ � �������� Api
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = $"{Assembly.GetExecutingAssembly().GetName().Name}",
        Description = "Web Api ��� ���������� LearnApp",
        Contact = new OpenApiContact
        {
            Name = "nuadolos",
            Url = new Uri("https://vk.com/nuadolos")
        }
    });

    // ��������� ������������ � ��������� Swagger
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(Path.Combine(AppContext.BaseDirectory, xmlFilename)));
});

builder.Services.AddCors();

// ��������� � ������������� ����� URL � ������ ��������
builder.Services.AddRouting(options => options.LowercaseUrls = true);

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

//��������� ������� �������� ���������� DI
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //���������� ������� LearnContext �� ����������
    var context = services.GetRequiredService<LearnContext>();

    //����� �������������� ������
    Initializer.RecreateDatabase(context);
    await Initializer.InitializeData(context);
}

app.Run();
