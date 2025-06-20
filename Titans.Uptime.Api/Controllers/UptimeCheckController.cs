using Microsoft.AspNetCore.Mvc;
using Titans.Uptime.Application.Interfaces;
using Titans.Uptime.Domain.Contracts;
using Titans.Uptime.Domain.Models;

namespace Titans.Uptime.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UptimeCheckController : ControllerBase
    {
        private readonly IUptimeCheckService _uptimeCheckService;
        private readonly ILogger<UptimeCheckController> _logger;

        public UptimeCheckController(IUptimeCheckService uptimeCheckService, ILogger<UptimeCheckController> logger)
        {
            _uptimeCheckService = uptimeCheckService;
            _logger = logger;
        }

        /// <summary>
        /// Get all uptime checks
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UptimeCheckDto>>> GetUptimeChecks()
        {
            try
            {
                var uptimeChecks = await _uptimeCheckService.GetAllAsync();
                return Ok(uptimeChecks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving uptime checks");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get only active uptime checks
        /// </summary>
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<UptimeCheckDto>>> GetActiveUptimeChecks()
        {
            try
            {
                var uptimeChecks = await _uptimeCheckService.GetActiveAsync();
                return Ok(uptimeChecks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active uptime checks");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a specific uptime check by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UptimeCheckDto>> GetUptimeCheck(int id)
        {
            try
            {
                var uptimeCheck = await _uptimeCheckService.GetByIdAsync(id);
                if (uptimeCheck == null)
                    return NotFound($"Uptime check with ID {id} not found");

                return Ok(uptimeCheck);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving uptime check {UptimeCheckId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new uptime check
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<UptimeCheckDto>> CreateUptimeCheck([FromBody] CreateUptimeCheckRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var uptimeCheck = await _uptimeCheckService.CreateAsync(request);
                return CreatedAtAction(nameof(GetUptimeCheck), new { id = uptimeCheck.Id }, uptimeCheck);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating uptime check");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Update an existing uptime check
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UptimeCheckDto>> UpdateUptimeCheck(int id, [FromBody] CreateUptimeCheckRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var uptimeCheck = await _uptimeCheckService.UpdateAsync(id, request);
                if (uptimeCheck == null)
                    return NotFound($"Uptime check with ID {id} not found");

                return Ok(uptimeCheck);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating uptime check {UptimeCheckId}", id);
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Delete an uptime check
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUptimeCheck(int id)
        {
            try
            {
                var result = await _uptimeCheckService.DeleteAsync(id);
                if (!result)
                    return NotFound($"Uptime check with ID {id} not found");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting uptime check {UptimeCheckId}", id);
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
