using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;

namespace Titans.Uptime.Persistence
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var context = new UptimeMonitorContext(
                serviceProvider.GetRequiredService<DbContextOptions<UptimeMonitorContext>>());

            if (context.Systems.Any()) return; // No duplicar seeds

            // 1. Sistemas
            var sistemas = new[]
            {
                new SystemEntity { Name = "Web Corporativa", Description = "Sitio principal de la empresa" },
                new SystemEntity { Name = "API de Pagos", Description = "Procesa las transacciones de clientes" },
                new SystemEntity { Name = "Intranet", Description = "Portal interno para empleados y RRHH" },
                new SystemEntity { Name = "App Móvil", Description = "Aplicación de clientes" }
            };
            context.Systems.AddRange(sistemas);

            // 2. Componentes
            var componentes = new[]
            {
                new Component { Name = "Frontend", Description = "Interfaz web", System = sistemas[0] },
                new Component { Name = "Backend", Description = "API principal", System = sistemas[0] },
                new Component { Name = "Pasarela", Description = "Conexión a bancos", System = sistemas[1] },
                new Component { Name = "Login", Description = "Módulo de autenticación", System = sistemas[2] },
                new Component { Name = "Notificaciones", Description = "Servicio de mensajería", System = sistemas[3] }
            };
            context.Components.AddRange(componentes);

            // 3. UptimeChecks
            var uptimeChecks = new[]
            {
                new UptimeCheck
                {
                    Name = "Landing Page",
                    CheckUrl = "https://corporativa.ejemplo.com/",
                    CheckInterval = 60,
                    System = sistemas[0],
                    Component = componentes[0],
                    AlertEmails = "soporte@ejemplo.com,devops@ejemplo.com",
                    IsActive = true,
                    CheckType = CheckType.Https,
                    CheckTimeout = 10
                },
                new UptimeCheck
                {
                    Name = "API Status",
                    CheckUrl = "https://corporativa.ejemplo.com/api/status",
                    CheckInterval = 120,
                    System = sistemas[0],
                    Component = componentes[1],
                    AlertEmails = "devs@ejemplo.com",
                    IsActive = true,
                    CheckType = CheckType.Https,
                    CheckTimeout = 8
                },
                new UptimeCheck
                {
                    Name = "Pasarela Bancaria",
                    CheckUrl = "https://pagos.ejemplo.com/pasarela/health",
                    CheckInterval = 90,
                    System = sistemas[1],
                    Component = componentes[2],
                    AlertEmails = "finanzas@ejemplo.com",
                    IsActive = true,
                    CheckType = CheckType.Https,
                    CheckTimeout = 12
                },
                new UptimeCheck
                {
                    Name = "Autenticación Empleados",
                    CheckUrl = "https://intranet.ejemplo.com/login/health",
                    CheckInterval = 180,
                    System = sistemas[2],
                    Component = componentes[3],
                    AlertEmails = "it@ejemplo.com",
                    IsActive = true,
                    CheckType = CheckType.Https,
                    CheckTimeout = 6
                },
                new UptimeCheck
                {
                    Name = "Push Notifications",
                    CheckUrl = "https://appmovil.ejemplo.com/notify/health",
                    CheckInterval = 150,
                    System = sistemas[3],
                    Component = componentes[4],
                    AlertEmails = "soporte@ejemplo.com",
                    IsActive = false, // uno inactivo para variar
                    CheckType = CheckType.Https,
                    CheckTimeout = 10
                }
            };
            context.UptimeChecks.AddRange(uptimeChecks);

            // 4. UptimeEvents (eventos históricos y recientes, falsos positivos, programados)
            var now = DateTime.UtcNow;
            var events = new List<UptimeEvent>
            {
                // Caída inesperada reciente y recuperación
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[0],
                    EventType = EventType.Down,
                    StartTime = now.AddMinutes(-120),
                    EndTime = now.AddMinutes(-115),
                    ErrorMessage = "Timeout - sin respuesta de servidor",
                    ResponseTime = 0,
                    IsFalsePositive = false,
                    Category = EventCategory.External
                },
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[0],
                    EventType = EventType.Up,
                    StartTime = now.AddMinutes(-115),
                    EndTime = now.AddMinutes(-110),
                    ErrorMessage = null,
                    ResponseTime = 200,
                    IsFalsePositive = false,
                    Category = EventCategory.External
                },
                // Evento de mantenimiento programado
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[1],
                    EventType = EventType.Down,
                    StartTime = now.AddDays(-1).AddHours(-3),
                    EndTime = now.AddDays(-1).AddHours(-2.8),
                    ErrorMessage = "Mantenimiento programado",
                    ResponseTime = 0,
                    IsFalsePositive = false,
                    Category = EventCategory.Internal
                },
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[1],
                    EventType = EventType.Up,
                    StartTime = now.AddDays(-1).AddHours(-2.8),
                    EndTime = now.AddDays(-1).AddHours(-2.6),
                    ErrorMessage = null,
                    ResponseTime = 110,
                    IsFalsePositive = false,
                    Category = EventCategory.Internal
                },
                // Falso positivo (alarma que no fue real)
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[2],
                    EventType = EventType.Down,
                    StartTime = now.AddMinutes(-300),
                    EndTime = now.AddMinutes(-299),
                    ErrorMessage = "Error 503 - Falso positivo",
                    ResponseTime = 0,
                    IsFalsePositive = true,
                    Category = EventCategory.Internal
                },
                // Caída prolongada de autenticación
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[3],
                    EventType = EventType.Down,
                    StartTime = now.AddHours(-24),
                    EndTime = now.AddHours(-23),
                    ErrorMessage = "Error 401 - Fallo de login",
                    ResponseTime = 0,
                    IsFalsePositive = false,
                    Category = EventCategory.External
                },
                new UptimeEvent {
                    UptimeCheck = uptimeChecks[3],
                    EventType = EventType.Up,
                    StartTime = now.AddHours(-23),
                    EndTime = now.AddHours(-22.5),
                    ErrorMessage = null,
                    ResponseTime = 100,
                    IsFalsePositive = false,
                    Category = EventCategory.External
                }
            };
            context.UptimeEvents.AddRange(events);

            context.SaveChanges();
        }
    }
}
