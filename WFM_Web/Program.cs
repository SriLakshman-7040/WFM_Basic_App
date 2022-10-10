using Microsoft.EntityFrameworkCore;
using WFM_Core.Abstraction;
using WFM_Domain.Models;
using WFM_Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(opt => opt.IdleTimeout = TimeSpan.FromDays(1));
builder.Services.AddDbContext<WfmDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WfmConStr")));
//builder.Services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=LogInPage}/{id?}");

app.Run();
