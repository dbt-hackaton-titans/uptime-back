using Microsoft.AspNetCore.Mvc;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : ControllerBase
    {
        private readonly ISystemService _systemService;
        private readonly ILogger<SystemController> _logger;

        public SystemController(ISystemService systemService, ILogger<SystemController> logger)
        {
            _systemService = systemService;
            _logger = logger;
        }

        /// <summary>
        /// Get all systems with their components
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemDto>>> GetSystems()
        {
            try
            {
                var systems = await _systemService.GetAllAsync();
                return Ok(systems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving systems");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific system by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<SystemDto>> GetSystem(int id)
        {
            try
            {
                var system = await _systemService.GetByIdAsync(id);
                if (system == null)
                    return NotFound($"System with ID {id} not found");

                return Ok(system);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving system {SystemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new system
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<SystemDto>> CreateSystem([FromBody] CreateSystemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var system = await _systemService.CreateAsync(request);
                return CreatedAtAction(nameof(GetSystem), new { id = system.Id }, system);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating system");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing system
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<SystemDto>> UpdateSystem(int id, [FromBody] CreateSystemRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var system = await _systemService.UpdateAsync(id, request);
                if (system == null)
                    return NotFound($"System with ID {id} not found");

                return Ok(system);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating system {SystemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a system
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSystem(int id)
        {
            try
            {
                var result = await _systemService.DeleteAsync(id);
                if (!result)
                    return NotFound($"System with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting system {SystemId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
