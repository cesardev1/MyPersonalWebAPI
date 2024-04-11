using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MyPersonalWebAPI.Auth;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services;
using MyPersonalWebAPI.Services.Auth;
using MyPersonalWebAPI.Services.JWT;
using MyPersonalWebAPI.Services.OpenIA.ChatGPT;
using MyPersonalWebAPI.Services.Roles;
using MyPersonalWebAPI.Services.Users;
using MyPersonalWebAPI.Services.WhatsappClound;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;
using MyPersonalWebAPI.Util;
using Npgsql.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add secrets options

builder.Services.Configure<SecretsOptions>(builder.Configuration.GetSection("Secrets"));

builder.Services.Configure<SecretsOptions>(options =>
{
    options.AccessTokenWhatsapp = Environment.GetEnvironmentVariable("ACCESS_TOKEN_WHATSAPP");
    options.ApiKeyWhatsapp = Environment.GetEnvironmentVariable("API_KEY_WHATSAPP");
    options.WhatsappPhoneId = Environment.GetEnvironmentVariable("WHATSAPP_PHONE_ID");
    options.ApiKeyOpenIA = Environment.GetEnvironmentVariable("API_KEY_OPEN_IA");
    options.JWTSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
    options.PostgreConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
});

// cors configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});


// Database services

builder.Services.AddScoped<IRolesServices, RolesServices>();
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddScoped<IWhatsAppMessageRepository, WhatsAppMessageRepository>();

// Add services to the container.
builder.Services.AddScoped<IWhatsappCloudSendMessageServices, WhatsappCloudSendMessageServices>();
builder.Services.AddScoped<IJWTServices, JWTServices>();
builder.Services.AddScoped<IWhatsAppMessageHandler, WhatsAppMessageHandler>();
builder.Services.AddScoped<WhatsAppUtilities>();
builder.Services.AddSingleton<IUtil, Util>();
builder.Services.AddSingleton<IChatGPTServices, ChatGPTServices>();
builder.Services.AddScoped<ApiKeyUtilities>();
builder.Services.AddScoped<IApiKeyRepository, ApiKeyRepository>();
builder.Services.AddScoped<IApiKeyManager, ApiKeyManager>();
builder.Services.AddScoped<IUserManager, UserManager>();


builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")));
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"));
});

// Add services to Auth
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET_KEY")))
    };
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    //title
    c.SwaggerDoc("v1", new() { Title = "MyPersonalWebAPI", Version = "v1" });

    // Add JWT Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement{
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors();

app.MapGet("/", () => "It's work");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
