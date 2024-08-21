using Backstage.Data;
using Backstage.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<VideoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VideoDB")));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

//�n�J�n�X�����]�p
// �]�m Session �A��
builder.Services.AddDistributedMemoryCache(); // �]�m���s���G���֨�
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // �]�m Session �L���ɶ�
    options.Cookie.HttpOnly = true; // �u���\ HTTP �s��
    options.Cookie.IsEssential = true; // ������ Cookie
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // �n�J����
        options.LogoutPath = "/Account/Logout"; // �n�X����
        options.AccessDeniedPath = "/Account/AccessDenied"; // �L�v������
    });
builder.Services.AddAuthorization();


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

app.UseAuthorization();

//jarry add
app.UseSession(); // �ҥ� Session �����n��
app.UseAuthentication(); // �T�O�K�[�F�o��
app.UseAuthorization();  // �T�O�K�[�F�o��

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
