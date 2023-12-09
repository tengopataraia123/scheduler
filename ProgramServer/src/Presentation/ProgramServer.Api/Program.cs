using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProgramServer.Application.Common.Mappings;
using ProgramServer.Application.Middlewares;
using ProgramServer.Application.Repository;
using ProgramServer.Application.Services.Bluetooth;
using ProgramServer.Application.Services.Events;
using ProgramServer.Application.Services.RequestDecrypt;
using ProgramServer.Application.Services.Roles;
using ProgramServer.Application.Services.Subjects;
using ProgramServer.Application.Services.Surveys;
using ProgramServer.Application.Services.Users;
using ProgramServer.Persistence.Data;
using ProgramServer.Persistence.Repository;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using ProgramServer.Application.Services.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("default")));
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBluetoothService, BluetoothService>();
builder.Services.AddScoped<ISubjectService, SubjectService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISurveyService, SurveyService>();
builder.Services.AddScoped<IResponseService, ResponseService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton<IRequestDecryptService, RequestDecryptService>();

builder.Services.AddMemoryCache();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers();


builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer",optons =>
    {
        var value = builder.Configuration.GetSection("AppSettings:Secret").Value;
        if (value != null)
            optons.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(value)),
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                RequireExpirationTime = false
            };
    });
#region Swagger Dependencies

builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MainServer",
        Description = "ASP.NET Core Web API"
    });

});
#endregion
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();
#if !DEBUG
app.UseMiddleware<ExceptionMiddleware>();
#endif

//app.UseHttpsRedirection();
app.UseWhen(o => o.Request.Headers.Keys.Contains("RsaEncrypted"), app => app.UseMiddleware<RequestDecryptionMiddleware>());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();