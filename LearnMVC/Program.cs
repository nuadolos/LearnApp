using LearnAPI.Validate;
using LearnEF.Context;
using LearnEF.Entities.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<LearnContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("LearnConnection")));

builder.Services.AddControllersWithViews();

//Добавление собственных валидаторов для аутентификации
//builder.Services.AddTransient<IUserValidator<User>, CustomUserValidator>();
//builder.Services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>(
//    service => new CustomPasswordValidator(6));

//Параметры проверки пароля
builder.Services.AddIdentity<User, IdentityRole>(
//    options =>
//{
//    options.Password.RequiredLength = 10;
//    options.Password.RequiredUniqueChars = 0;
//    options.Password.RequireLowercase = false;
//    options.Password.RequireUppercase = false;
//    options.Password.RequireDigit = false;
//    options.Password.RequireNonAlphanumeric = false;

//    options.Lockout.MaxFailedAccessAttempts = 5;
//    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//    options.Lockout.AllowedForNewUsers = true;
//}
)
    .AddEntityFrameworkStores<LearnContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
