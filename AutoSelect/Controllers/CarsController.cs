using AutoSelect.Data;
using AutoSelect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace AutoSelect.Controllers
{
    [Route("api/cars")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/cars
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _context.Cars.ToListAsync();
            if (!cars.Any())
            {
                return NotFound("Автомобілів не знайдено.");
            }
            return Ok(cars);
        }

        // GET: api/cars/{id}
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound("Автомобіль не знайдено.");
            }
            return Ok(car);
        }

        // POST: api/cars/create
        // Замінив [FromQuery] на [FromBody] і додав DTO для кращої організації
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCar([FromBody] CreateCarModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var clientExists = await _context.Clients.AnyAsync(c => c.Id == model.ClientId);
            if (!clientExists)
            {
                return BadRequest("Клієнта з вказаним ID не знайдено.");
            }

            var car = new Car
            {
                ClientId = model.ClientId,
                Model = model.Model,
                LicensePlate = model.LicensePlate,
                LocationLat = model.LocationLat ?? 0.0,
                LocationLng = model.LocationLng ?? 0.0
            };

            _context.Cars.Add(car);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCar), new { id = car.Id }, car);
        }
    }

    // DTO для створення авто
    public class CreateCarModel
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Model { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLng { get; set; }
    }
}
