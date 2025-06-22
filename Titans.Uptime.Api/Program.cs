using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.Json.Serialization;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Application.Services;
using Titans.Uptime.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Database
builder.Services.AddDbContext<UptimeMonitorContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ??
                     "Data Source=uptimemonitor.db"));


// Services
builder.Services.AddScoped<ISystemService, SystemService>();
builder.Services.AddScoped<IComponentService, ComponentService>();
builder.Services.AddScoped<IUptimeCheckService, UptimeCheckService>();
builder.Services.AddScoped<IUptimeEventService, UptimeEventService>();


// Configuración SMTP
var smtpHost = builder.Configuration["Smtp:Host"] ?? "sandbox.smtp.mailtrap.io";
var smtpPort = int.Parse(builder.Configuration["Smtp:Port"] ?? "587");
var smtpUser = builder.Configuration["Smtp:User"] ?? "30698baa9fb4ef";
var smtpPass = builder.Configuration["Smtp:Pass"] ?? "8405f7aef7c022";
var smtpFrom = builder.Configuration["Smtp:From"] ?? "titans@uptime.com";

builder.Services.AddScoped<SmtpClient>(_ => new SmtpClient(smtpHost, smtpPort)
{
    Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass),
    EnableSsl = true
});
builder.Services.AddScoped<IEmailService>(provider =>
    new EmailService(provider.GetRequiredService<SmtpClient>(), smtpFrom)
);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Uptime Monitor API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<UptimeMonitorContext>();
    context.Database.EnsureCreated();
}

app.Run();
