// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
// using GrammyAwards.Data;
// using System;

// var builder = WebApplication.CreateBuilder(args);

// //Add Pomelo DbContext with MySQL
// builder.Services.AddControllers();

// builder.Services.AddDbContext<AppContext>(options =>
//     options.UseMySql(
//         builder.Configuration.GetConnectionString("DefaultConnection"),
//         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
//     )
// );

// internal class ServerVersion
// {
//     internal static object AutoDetect(string? v)
//     {
//         throw new NotImplementedException();
//     }
// }

// // 取得連線字串，確保存在
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
//     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// // 設定 MySQL 連線
// var serverVersion = new MySqlServerVersion(new Version(8, 0, 28));

// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseMySql(connectionString, serverVersion));

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// // 設定 Identity
// builder.Services.AddDefaultIdentity<IdentityUser>(options => 
//     options.SignIn.RequireConfirmedAccount = true)
//     .AddEntityFrameworkStores<ApplicationDbContext>();

// // 加入 MVC
// builder.Services.AddControllersWithViews();

// var app = builder.Build();

// // 設定請求處理流程
// if (app.Environment.IsDevelopment())
// {
//     app.UseMigrationsEndPoint();
// }
// else
// {
//     app.UseExceptionHandler("/Home/Error");
//     app.UseHsts();
// }

// app.UseHttpsRedirection();
// app.UseRouting();
// app.UseAuthorization();

// app.MapStaticAssets();

// app.MapControllerRoute(
//     name: "default",
//     pattern: "{controller=Home}/{action=Index}/{id?}")
//     .WithStaticAssets();

// app.MapRazorPages()
//    .WithStaticAssets();

// app.Run();
// using Microsoft.EntityFrameworkCore;
using System;
using GrammyAwards.Data;
using GrammyAwards.Models;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();

// Add Pomelo DbContext with MySQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Register IPlayerService and PlayerService for dependency injection
// builder.Services.AddScoped<IPlayerService, PlayerService>();
// builder.Services.AddScoped<IEntityService<T>, PlayerService>();
// builder.Services.AddScoped(typeof(IEntityService<>), typeof(PlayerService<>));
// builder.Services.AddScoped(typeof(IEntityService<>), typeof(PlayerService<>));
// builder.Services.AddScoped(typeof(IEntityService<>), typeof(EntityService<>));


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


// Test Database Connection
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Check if connection is successful
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


// app.MapGet("/", () => "Welcome to the Player API!");


// Map controllers (this is where the PlayerController comes into play)
app.MapControllers();

app.Run();