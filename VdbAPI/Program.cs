using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

using VdbAPI.hubs;
using VdbAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<VideoDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("VideoDB")));

builder.Services.AddCors(options => {
    options.AddPolicy("AllowSpecificOrigin",builder =>
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials()
    );
});

builder.Services.AddSignalR();

builder.Services.AddHttpClient();

builder.Services.AddControllersWithViews();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();
app.UseCors("AllowSpecificOrigin");

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles(new StaticFileOptions {
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath,"img")),
    RequestPath = "/img"
});
app.UseHttpsRedirection();
app.MapHub<ChatHub>("/chathub");
app.UseAuthorization();

app.MapControllers();

app.Run();
