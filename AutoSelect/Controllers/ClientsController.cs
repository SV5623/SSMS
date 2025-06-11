using AutoSelect.Data;
using AutoSelect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AutoSelect.Controllers.Api;

[Route("api/clients")]
[ApiController]
public class ClientsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ClientsController(AppDbContext context)
    {
        _context = context;
    }

    // DTO для створення клієнта

    public record CreateClientDto
    {
        [Required(ErrorMessage = "Ім’я клієнта є обов’язковим.")]
        [StringLength(100, ErrorMessage = "Ім’я не може перевищувати 100 символів.")]
        public string Name { get; init; }

        [StringLength(20, ErrorMessage = "Телефон не може перевищувати 20 символів.")]
        public string? Phone { get; init; }
    }


        // GET: api/clients
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            var clients = await _context.Clients.ToListAsync();
            if (!clients.Any())
            {
                return NotFound("Клієнтів не знайдено.");
            }
            return Ok(clients);
        }

        // GET: api/clients/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound("Клієнта не знайдено.");
            }
            return Ok(client);
        }

        // POST: api/clients
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var client = new Client
            {
                Name = dto.Name,
                Phone = dto.Phone
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }
}