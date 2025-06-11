using System.Text.Json;
using AutoSelect.Data;
using AutoSelect.Models;
using Microsoft.AspNetCore.Mvc;


namespace AutoSelect.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReportsController> _logger;

        public ReportsController(AppDbContext context , ILogger<ReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

[HttpPost("create")]
[ProducesResponseType(StatusCodes.Status201Created)]
[ProducesResponseType(StatusCodes.Status400BadRequest)]
public async Task<IActionResult> CreateReport([FromBody] CreateReportModel dto)
{
    _logger.LogInformation($"Received report creation request: {JsonSerializer.Serialize(dto)}");

    if (dto == null)
    {
        _logger.LogWarning("Request body is null.");
        return BadRequest("Дані запиту відсутні.");
    }

    if (string.IsNullOrWhiteSpace(dto.Description))
    {
        _logger.LogWarning("Description is empty.");
        return BadRequest("Опис звіту є обов’язковим.");
    }

    var task = await _context.Tasks.FindAsync(dto.TaskId);
    if (task == null)
    {
        _logger.LogWarning($"Task with ID {dto.TaskId} not found.");
        return BadRequest($"Завдання з ID {dto.TaskId} не знайдено.");
    }

    var report = new Models.Report
    {
        TaskId = dto.TaskId,
        Description = dto.Description.Trim(),
        CompletedAt = DateTime.UtcNow
    };

    _context.Reports.Add(report);
    await _context.SaveChangesAsync();

    _logger.LogInformation($"Report created with ID {report.Id}");
    return CreatedAtAction(nameof(GetReport), new { id = report.Id }, report);
}

// DTO for creating a report
public class CreateReportModel
{
    public int TaskId { get; set; }
    public string Description { get; set; }
}

        // GET: api/reports/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetReport(int id)
        {
            var report = await _context.Reports.FindAsync(id);
            if (report == null)
            {
                return NotFound("Звіт не знайдено.");
            }
            return Ok(report);
        }
    }
}