using Microsoft.AspNetCore.Mvc;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Models;
using Titans.Uptime.Domain;

namespace Titans.Uptime.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UptimeEventsController : ControllerBase
    {
        private readonly IUptimeEventService _eventService;

        public UptimeEventsController(IUptimeEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UptimeEventDto>>> GetAll()
        {
            var events = await _eventService.GetAllAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UptimeEventDto>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser un número positivo.");

            var ev = await _eventService.GetByIdAsync(id);
            if (ev == null)
                return NotFound($"No se encontró el evento con ID {id}.");
            return Ok(ev);
        }

        [HttpGet("by-check/{uptimeCheckId}")]
        public async Task<ActionResult<IEnumerable<UptimeEventDto>>> GetByCheckId(int uptimeCheckId)
        {
            if (uptimeCheckId <= 0)
                return BadRequest("El ID del UptimeCheck debe ser positivo.");

            var events = await _eventService.GetByCheckIdAsync(uptimeCheckId);
            return Ok(events);
        }

        [HttpPost]
        public async Task<ActionResult<UptimeEventDto>> Create([FromBody] UptimeEventDto dto)
        {
            if (dto == null)
                return BadRequest("El cuerpo de la solicitud no puede estar vacío.");

            // Ejemplo de validaciones mínimas de campos
            if (dto.UptimeCheckId <= 0)
                return BadRequest("Debe indicar un UptimeCheckId válido.");

            if (dto.StartTime == default)
                return BadRequest("Debe indicar la fecha de inicio (StartTime).");

            // Puedes agregar más validaciones según tus reglas de negocio

            var created = await _eventService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPatch("{id}/mark-false-positive")]
        public async Task<IActionResult> MarkAsFalsePositive(int id, [FromQuery] bool isFalsePositive = true)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser positivo.");

            var evento = await _eventService.GetByIdAsync(id);
            if (evento == null)
                return NotFound($"No se encontró el evento con ID {id}.");

            await _eventService.MarkAsFalsePositiveAsync(id, isFalsePositive);
            return NoContent();
        }

        [HttpPatch("{id}/categorize")]
        public async Task<IActionResult> CategorizeEvent(int id, [FromQuery] EventCategory category)
        {
            if (id <= 0)
                return BadRequest("El ID debe ser positivo.");

            // Validar que el valor de category sea un valor válido del enum
            if (!Enum.IsDefined(typeof(EventCategory), category))
                return BadRequest("Categoría de evento inválida.");

            var evento = await _eventService.GetByIdAsync(id);
            if (evento == null)
                return NotFound($"No se encontró el evento con ID {id}.");

            await _eventService.CategorizeEventAsync(id, category);
            return NoContent();
        }
    }
}
