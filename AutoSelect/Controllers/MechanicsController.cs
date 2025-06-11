using AutoSelect.Data;
using AutoSelect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoSelect.Controllers
{
    [Route("api/mechanics")]
    [ApiController]
    public class MechanicsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MechanicsController> _logger;

        public MechanicsController(AppDbContext context, ILogger<MechanicsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/mechanics
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Mechanic>>> GetMechanics()
        {
            var mechanics = await _context.Mechanics.Where(m => m.IsAvailable).ToListAsync();
            if (!mechanics.Any())
            {
                return NotFound("Немає вільних механіків.");
            }
            return Ok(mechanics);
        }

        // GET: api/mechanics/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMechanicById(int id)
        {
            var mechanic = await _context.Mechanics.FindAsync(id);
            if (mechanic == null)
            {
                return NotFound("Механіка не знайдено.");
            }
            return Ok(mechanic);
        }

        // POST: api/mechanics/create
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddMechanic([FromBody] CreateMechanicModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mechanic = new Mechanic
            {
                Name = model.Name,
                IsAvailable = model.IsAvailable
            };

            _context.Mechanics.Add(mechanic);
            _logger.LogCritical("===============================WORKING======================");
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMechanicById), new { id = mechanic.Id }, mechanic);
        }
    }

    // DTO for creating a mechanic
    public record CreateMechanicModel
    {
        [Required(ErrorMessage = "Ім’я механіка є обов’язковим.")]
        [StringLength(100)]
        public string Name { get; init; }

        public bool IsAvailable { get; init; } = true; // за замовчуванням вільний
    }
}
