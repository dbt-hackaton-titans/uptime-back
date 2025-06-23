using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromAddress;

        public EmailService(SmtpClient smtpClient, string fromAddress)
        {
            _smtpClient = smtpClient;
            _fromAddress = fromAddress;
        }

        public async Task SendAlertAsync(string[] recipients, string subject, string message)
        {
            if (recipients == null || recipients.Length == 0)
                throw new ArgumentException("Debe proporcionar al menos un destinatario.");

            using var mailMessage = new MailMessage()
            {
                From = new MailAddress(_fromAddress),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };

            foreach (var recipient in recipients.Where(r => !string.IsNullOrWhiteSpace(r)))
            {
                mailMessage.To.Add(recipient);
            }

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendDownAlertAsync(UptimeCheckDto uptimeCheck, UptimeEvent downEvent)
        {
            // Aquí defines el subject y el mensaje según tus reglas de monitoreo
            var subject = $"ALERTA: {uptimeCheck.Name} está CAÍDO ({downEvent.StartTime:g})";
            var message =
                        $@"Hola,
                        
                        El servicio/endpoint '{uptimeCheck.Name}' ({uptimeCheck.CheckUrl}) está CAÍDO.
                        
                        Detalles:
                        - Hora de caída: {downEvent.StartTime:g}
                        - Error: {downEvent.ErrorMessage ?? "No especificado"}
                        - Tipo de evento: {downEvent.EventType}
                        - Id del UptimeCheck: {uptimeCheck.Id}
                        
                        Por favor, investigue la causa del incidente.
                        
                        -- Uptime Titans Monitor";

            var recipients = GetAlertEmails(uptimeCheck);
            await SendAlertAsync(recipients, subject, message);
        }

        public async Task SendUpAlertAsync(UptimeCheckDto uptimeCheck, UptimeEvent upEvent)
        {
            var subject = $"RECUPERADO: {uptimeCheck.Name} está ARRIBA ({upEvent.StartTime:g})";
            var duration = upEvent.EndTime.HasValue && upEvent.StartTime != default(DateTime)
                ? (upEvent.EndTime.Value - upEvent.StartTime).ToString(@"hh\:mm\:ss")
                : "No disponible";

            var message =
                            $@"Hola,
                            
                            El servicio/endpoint '{uptimeCheck.Name}' ({uptimeCheck.CheckUrl}) se ha RECUPERADO.
                            
                            Detalles:
                            - Hora de recuperación: {upEvent.StartTime:g}
                            - Tiempo caído (aprox): {duration}
                            - Id del UptimeCheck: {uptimeCheck.Id}
                            
                            Puede seguir monitoreando el estado en el dashboard.
                            
                            -- Uptime Titans Monitor";

            var recipients = GetAlertEmails(uptimeCheck);
            await SendAlertAsync(recipients, subject, message);
        }

        /// <summary>
        /// Método auxiliar para obtener los emails de alerta del UptimeCheck.
        /// Ajusta esto según cómo guardes los emails (separados por ; o , en la propiedad AlertEmails).
        /// </summary>
        private string[] GetAlertEmails(UptimeCheckDto uptimeCheck)
        {
            if (string.IsNullOrWhiteSpace(uptimeCheck.AlertEmails))
                return Array.Empty<string>();
            // Soporta separación por ',' o ';'
            return uptimeCheck.AlertEmails
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToArray();
        }
    }
}
