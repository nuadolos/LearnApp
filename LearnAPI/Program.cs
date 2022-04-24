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

#region ���������������� ���������� ��� ��������� ������������

builder.Services.AddDbContextPool<LearnContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnConnection"),
        o => o.EnableRetryOnFailure()));

builder.Services.AddControllersWithViews();

//���������� UserRepo � RoleRepo � ��������� DI
builder.Services.AddScoped<ILearnRepo, LearnRepo>();
builder.Services.AddScoped<ISourceLoreRepo, SourceLoreRepo>();

#endregion

#region ���������� ������� Identity

//���������� ����������� ����������� ��� ��������������
builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();
builder.Services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>(
    service => new CustomPasswordValidator(6));

//��������� �������� ������
builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 0;
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireDigit = false;
        options.Password.RequireNonAlphanumeric = false;

        options.Lockout.MaxFailedAccessAttempts = 5;
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

//����������� ������� ���������� � ����������� � ����� Pascal
builder.Services.AddMvc().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//������������� �������������� � �����������
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//��������� ������� �������� ���������� DI
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    //���������� ������� LearnContext, UserManager � RoleManager �� ����������
    var context = services.GetRequiredService<LearnContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    //����� �������������� ������
    Initializer.RecreateDatabase(context);
    await Initializer.InitializeData(context, userManager, roleManager);
}

app.Run();
