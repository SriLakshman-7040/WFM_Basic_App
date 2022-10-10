using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using WFM_API.Models;
using WFM_Core.Abstraction;
using WFM_Domain.Models;
using WFM_Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<WfmDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("WfmConStr")));
var _dbcontext = builder.Services.BuildServiceProvider().GetService<WfmDbContext>();
builder.Services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator(_dbcontext));
builder.Services.AddScoped<IManagerStuff, ManagerService>();
builder.Services.AddScoped<IMembersStuff, MemberService>();

var _jwtsetting = builder.Configuration.GetSection("JWTSetting");
builder.Services.Configure<JWTSetting>(_jwtsetting);

var authkey = builder.Configuration.GetValue<string>("JWTSetting:securitykey");
builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
        ValidateIssuer = false,
        ValidateAudience = false

    };
});
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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
