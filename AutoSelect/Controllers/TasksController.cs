using AutoSelect.Data;
using AutoSelect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace AutoSelect.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<TasksController> _logger;

        public TasksController(AppDbContext context, ILogger<TasksController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Models.Task>>> GetTasks()
        {
            var tasks = await _context.Tasks
                .Include(t => t.Mechanic)
                .Include(t => t.Car)
                .ToListAsync();

            if (!tasks.Any())
            {
                return NotFound("Завдань не знайдено.");
            }
            return Ok(tasks);
        }

        // GET: api/tasks/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _context.Tasks
                .Include(t => t.Mechanic)
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
            {
                return NotFound("Завдання не знайдено.");
            }
            return Ok(task);
        }

        // [HttpPost("create")]
        // [ProducesResponseType(StatusCodes.Status201Created)]
        // [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // public async Task<IActionResult> CreateTask(int carId, string description, int? mechanicId = null)
        // {

        //     // Перевірка автомобіля
        //     var car = await _context.Cars.FindAsync(carId);
        //     if (car == null)
        //         return BadRequest($"Автомобіль з ID {carId} не знайдено.");

        //     // Пошук механіка
        //     Mechanic mechanic = null;

        //     if (mechanicId is int mechId)
        //     {
        //         mechanic = await _context.Mechanics.FindAsync(mechId);
        //         if (mechanic == null)
        //             return BadRequest($"Механіка з ID {mechId} не знайдено.");

        //         if (!mechanic.IsAvailable)
        //             return BadRequest($"Механік з ID {mechId} наразі зайнятий.");
        //     }
        //     else
        //     {
        //         mechanic = await _context.Mechanics.FirstOrDefaultAsync(m => m.IsAvailable);
        //         if (mechanic == null)
        //             return BadRequest("Немає доступних механіків.");
        //     }


        //     _logger.LogCritical("===============================WORKING======================");

        //     // Створення завдання
        //     var task = new Models.Task
        //     {
        //         CarId = carId,
        //         MechanicId = mechanic.Id,
        //         Description = description?.Trim(),
        //         Status = "Pending",
        //         CreatedAt = DateTime.UtcNow
        //     };

        //     mechanic.IsAvailable = false;

        //     _context.Tasks.Add(task);
        //     await _context.SaveChangesAsync();

        //     return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        // }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskModel dto)
        {
            _logger.LogInformation($"Received task creation request: {JsonSerializer.Serialize(dto)}");

            // Валідація вхідних даних
            if (dto == null)
            {
                _logger.LogWarning("Request body is null.");
                return BadRequest("Дані запиту відсутні.");
            }

            if (string.IsNullOrWhiteSpace(dto.Description))
            {
                _logger.LogWarning("Description is empty.");
                return BadRequest("Опис завдання є обов’язковим.");
            }

            // Перевірка автомобіля
            var car = await _context.Cars.FindAsync(dto.CarId);
            if (car == null)
            {
                _logger.LogWarning($"Car with ID {dto.CarId} not found.");
                return BadRequest($"Автомобіль з ID {dto.CarId} не знайдено.");
            }

            // Пошук механіка
            Mechanic mechanic = null;
            if (dto.MechanicId.HasValue)
            {
                mechanic = await _context.Mechanics.FindAsync(dto.MechanicId.Value);
                if (mechanic == null)
                {
                    _logger.LogWarning($"Mechanic with ID {dto.MechanicId} not found.");
                    return BadRequest($"Механіка з ID {dto.MechanicId} не знайдено.");
                }
                if (!mechanic.IsAvailable)
                {
                    _logger.LogWarning($"Mechanic with ID {dto.MechanicId} is not available.");
                    return BadRequest($"Механік з ID {dto.MechanicId} наразі зайнятий.");
                }
            }
            else
            {
                mechanic = await _context.Mechanics.FirstOrDefaultAsync(m => m.IsAvailable);
                if (mechanic == null)
                {
                    _logger.LogWarning("No available mechanics found.");
                    return BadRequest("Немає доступних механіків.");
                }
            }

            _logger.LogInformation("Creating task...");

            // Створення завдання
            var task = new Models.Task
            {
                CarId = dto.CarId,
                MechanicId = mechanic.Id,
                Description = dto.Description.Trim(),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };

            mechanic.IsAvailable = false;

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Task created with ID {task.Id}");

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }


    }


    // DTO for creating a task
    public class CreateTaskModel
    {
        public int CarId { get; set; }
        public string Description { get; set; }
        public int? MechanicId { get; set; }
    }
}