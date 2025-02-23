using System;
using GrammyAwards.Data;
using GrammyAwards.Models;
using GrammyAwards.Interfaces;
using GrammyAwards.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddControllersWithViews();  // Add this only once

// Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Services
builder.Services.AddScoped<IArtistService, ArtistService>();
builder.Services.AddScoped<IAwardService, AwardService>();
builder.Services.AddScoped<ISongService, SongService>();
builder.Services.AddScoped<ISongArtistService, SongArtistService>();
builder.Services.AddScoped<ISongAwardService, SongAwardService>();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Razor Pages services
builder.Services.AddRazorPages();  // Ensure Razor Pages services are added

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            Console.WriteLine("Database connection successful!");
        }
        else
        {
            Console.WriteLine("Database connection failed.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error testing the database connection: {ex.Message}");
    }
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=ArtistPage}/{action=List}/{id?}");

app.MapControllers();
app.MapRazorPages();  // Ensure Razor Pages routes are mapped

app.Run();
