using MyPersonalWebAPI.Models;
using MyPersonalWebAPI.Services.WhatsappClound.SendMessage;
using MyPersonalWebAPI.Util;

var builder = WebApplication.CreateBuilder(args);

// Add secrets options

builder.Services.Configure<SecretsOptions>(builder.Configuration.GetSection("Secrets"));

builder.Services.Configure<SecretsOptions>(options =>
{
    options.AccessTokenWhatsapp = Environment.GetEnvironmentVariable("AccessTokenWhatsapp");
    options.ApiKeyWhatsapp = Environment.GetEnvironmentVariable("ApiKeyWhatsapp");
    options.WhatsappPhoneId = Environment.GetEnvironmentVariable("WhatsappPhoneId");
    options.ApiKeyOpenIA = Environment.GetEnvironmentVariable("ApiKeyOpenIA");
});

// Add services to the container.
builder.Services.AddSingleton<IWhatsappCloudSendMessageServices, WhatsappCloudSendMessageServices>();
builder.Services.AddSingleton<IUtil, Util>();

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
