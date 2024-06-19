using MeetingApp.Data.Abstract;
using MeetingApp.Data.Concrete;
using MeetingApp.Data.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IMeetingRepository, EFMeetingRepository>();
builder.Services.AddScoped<IUserRepository, EFUserRepository>();

builder.Services.AddDbContext<MeetingContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("mysql_connection"),
        new MySqlServerVersion(new Version(8, 0, 29))
    )
);

// Configure Identity
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<MeetingContext>()
    .AddDefaultTokenProviders();

// Configure authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

var app = builder.Build();

SeedData.TestVerileriniDoldur(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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
