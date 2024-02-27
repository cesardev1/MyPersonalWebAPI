using Microsoft.EntityFrameworkCore;
using MyPersonalWebAPI.Data;
using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.JWT;
using MyPersonalWebAPI.Services.OpenIA.ChatGPT;
using MyPersonalWebAPI.Services.Roles;
using MyPersonalWebAPI.Services.Users;
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

// Add services to the container.


builder.Services.AddSingleton<IWhatsappCloudSendMessageServices, WhatsappCloudSendMessageServices>();
builder.Services.AddScoped<IJWTServices,JWTServices>();
builder.Services.AddSingleton<IUtil, Util>();
builder.Services.AddSingleton<IChatGPTServices, ChatGPTServices>();
builder.Services.AddScoped<IRolesServices, RolesServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddDbContext<DatabaseContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING")));
var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
builder.Services.AddEntityFrameworkNpgsql().AddDbContext<DatabaseContext>(options =>
{
    options.UseNpgsql(Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING"));
});


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
