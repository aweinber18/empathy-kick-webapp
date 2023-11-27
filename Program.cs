using Azure.Identity;
using EmpathyKick;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var keyVaultEndpoint = new Uri(Environment.GetEnvironmentVariable("EmpathyDBConnection"));

builder.Configuration.AddAzureKeyVault(keyVaultEndpoint, new DefaultAzureCredential());

// Add services to the container.
builder.Services.AddControllersWithViews();
var connection = "Server=tcp:empathykick.database.windows.net,1433;Initial Catalog=EmpathyKickDB;Persist Security Info=False;User ID=empathyadmin;Password=Adminpassword123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"; 
//Environment.GetEnvironmentVariable("DefaultConnection");
var connection2 = builder.Configuration.GetConnectionString("EmpathyDBConnection");
builder.Services.AddDbContext<MyDBContext>(options => options.UseSqlServer(connection, options => options.EnableRetryOnFailure()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    Console.WriteLine("not dev");
    Console.WriteLine();
    Console.WriteLine();
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    Console.WriteLine();
    Console.WriteLine("dev");
    Console.WriteLine();
    Console.WriteLine();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
