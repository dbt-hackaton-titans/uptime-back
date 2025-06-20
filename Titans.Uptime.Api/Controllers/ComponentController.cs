using Microsoft.AspNetCore.Mvc;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComponentsController : ControllerBase
    {
        private readonly IComponentService _componentService;
        private readonly ILogger<ComponentsController> _logger;

        public ComponentsController(IComponentService componentService, ILogger<ComponentsController> logger)
        {
            _componentService = componentService;
            _logger = logger;
        }

        /// <summary>
        /// Get all components
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComponentDto>>> GetComponents()
        {
            try
            {
                var components = await _componentService.GetAllAsync();
                return Ok(components);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving components");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get components by system ID
        /// </summary>
        [HttpGet("system/{systemId}")]
        public async Task<ActionResult<IEnumerable<ComponentDto>>> GetComponentsBySystem(int systemId)
        {
            try
            {
                var components = await _componentService.GetBySystemIdAsync(systemId);
                return Ok(components);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving components for system {SystemId}", systemId);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific component by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ComponentDto>> GetComponent(int id)
        {
            try
            {
                var component = await _componentService.GetByIdAsync(id);
                if (component == null)
                    return NotFound($"Component with ID {id} not found");

                return Ok(component);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new component
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ComponentDto>> CreateComponent([FromBody] CreateComponentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var component = await _componentService.CreateAsync(request);
                return CreatedAtAction(nameof(GetComponent), new { id = component.Id }, component);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating component");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing component
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ComponentDto>> UpdateComponent(int id, [FromBody] CreateComponentRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var component = await _componentService.UpdateAsync(id, request);
                if (component == null)
                    return NotFound($"Component with ID {id} not found");

                return Ok(component);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete a component
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteComponent(int id)
        {
            try
            {
                var result = await _componentService.DeleteAsync(id);
                if (!result)
                    return NotFound($"Component with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting component {ComponentId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
