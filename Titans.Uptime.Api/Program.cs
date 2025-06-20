using Microsoft.EntityFrameworkCore;
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
