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


var secret = builder.Configuration.GetSection("AppSettings:Secret").Value;

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer("Bearer",optons =>
    {
        optons.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateAudience = false,
            RequireExpirationTime = false
        };
    });
#region Swagger Dependencies

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ProgramApi",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
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