using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using SignalR1.Hubs;
using SignalR1.Models;
using Microsoft.Extensions.Logging;
using SignalR1.myModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
// Add services for cookies.
builder.Services.AddHttpContextAccessor();

//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
//builder.Logging.AddConsole();

builder.Services.AddDbContext<MyChatDatabseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("myCS"))
);

builder.Services.AddScoped<Utility>();
builder.Services.AddScoped<HubConnectionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
app.UseRouting();

// Add Cookie Policy Middleware
app.UseCookiePolicy(new CookiePolicyOptions
{
    Secure = CookieSecurePolicy.Always,
    HttpOnly = HttpOnlyPolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Lax
});

app.UseAuthorization();

app.MapHub<ChatHub>("/chatHub");

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();