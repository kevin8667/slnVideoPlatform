using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System.Net.Http.Headers;
using System.Text.Json;
using VdbAPI.Models;

var builder = WebApplication.CreateBuilder(args);

#region line login
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Line";
})
.AddCookie()
.AddOAuth("Line", options =>
{
    options.ClientId = builder.Configuration["Authentication:Line:ClientId"];
    options.ClientSecret = builder.Configuration["Authentication:Line:ClientSecret"];
    options.AuthorizationEndpoint = "https://access.line.me/oauth2/v2.1/authorize";
    options.TokenEndpoint = "https://api.line.me/oauth2/v2.1/token";
    options.UserInformationEndpoint = "https://api.line.me/v2/profile";
    options.CallbackPath = "/auth/callback"; // 這裡設置 CallbackPath

    options.Scope.Add("profile");
    options.Scope.Add("openid");

    options.Events = new OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var httpClient = context.Backchannel;

            // Get user info
            var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
            var response = await httpClient.SendAsync(request, context.HttpContext.RequestAborted);
            response.EnsureSuccessStatusCode();

            var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync()).RootElement;

            context.RunClaimActions(user);
        }
    };
});
#endregion
// Add services to the container.

builder.Services.AddDbContext<VideoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VideoDB")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

});

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath,"img")),
    RequestPath = "/img"  // URL �e��
});
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
