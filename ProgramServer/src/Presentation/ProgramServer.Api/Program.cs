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
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

builder.Services.AddSingleton<IRequestDecryptService, RequestDecryptService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://localhost:3000/", "https://0.0.0.0:7137")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

});


builder.Services.AddAuthentication()
    .AddJwtBearer("Bearer",optons =>
    {
        optons.Authority = "https://accounts.google.com";
        optons.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            $"{optons.Authority}/.well-known/openid-configuration",
            new OpenIdConnectConfigurationRetriever(), new HttpDocumentRetriever());
        optons.RequireHttpsMetadata = false;
        optons.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = "https://accounts.google.com",
            ValidateIssuer = true,
            ValidAlgorithms = new[] { "RS256" },
            ValidAudience = "817166273814-css9qgkp0sgn8bpci0s24i7j9sonl7v2.apps.googleusercontent.com",
            ValidateAudience = true,
            ValidateLifetime = false,
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
    // To Enable authorization using Swagger (JWT)  
    swagger.AddSecurityDefinition("OAuth2", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                Scopes = new Dictionary<string, string>
                {
                    {"email","Email"}
                }
            }
        },
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "OAuth2"
                }
            },
            new string[] {"email"}
        }
    });

});
#endregion
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseMiddleware<ExceptionMiddleware>();

//app.UseHttpsRedirection();
app.UseWhen(o => o.Request.Headers.Keys.Contains("RsaEncrypted"), app => app.UseMiddleware<RequestDecryptionMiddleware>());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();