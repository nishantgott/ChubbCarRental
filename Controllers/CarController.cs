using Microsoft.AspNetCore.Mvc;
using ChubbCarRental.Model;
using ChubbCarRental.Repositories;
using Microsoft.AspNetCore.Authorization;
using ChubbCarRental.Services;
using System.Security.Claims;

namespace ChubbCarRental.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly MailgunEmailService _emailService;
        private readonly ICarRepositary _carRepositary;

        public CarController(ICarRepositary carRepositary, MailgunEmailService emailService)
        {
            _carRepositary = carRepositary;
            _emailService = emailService;
        }

        [HttpGet]
        public IActionResult GetAvailableCars()
        {
            var cars = _carRepositary.GetAvailableCars();
            return Ok(cars);
        }

        [HttpPost("rent")]
        [Authorize]
        public async Task<IActionResult> RentCar(int id)
        {
            var car = _carRepositary.GetCarById(id); 
            if (car == null)
            {
                return BadRequest("Car not found");
            }

            if (!car.IsAvailable)
            {
                return BadRequest("Car is not available");
            }

            car.IsAvailable = false;
            _carRepositary.UpdateCarAvailability(car.Id, car.IsAvailable);

            var emailSent = await _emailService.SendBookingConfirmationEmail("nishantgk2004@gmail.com", "nish", car.Make, car.Model);

            return Ok(new { Message = "Car rented successfully", Car = car });
        }


        [HttpPost]
        public IActionResult AddCar([FromBody] CarModel car)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _carRepositary.AddCar(car);
            return CreatedAtAction(nameof(GetAvailableCars), new { id = car.Id }, car);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateCar(int id, [FromBody] CarModel updatedCar)
        {
            var existingCar = _carRepositary.GetCarById(id);
            if (existingCar == null)
                return NotFound();

            existingCar.Make = updatedCar.Make;
            existingCar.Model = updatedCar.Model;
            existingCar.Year = updatedCar.Year;
            existingCar.PricePerDay = updatedCar.PricePerDay;
            existingCar.IsAvailable = updatedCar.IsAvailable;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            var car = _carRepositary.GetCarById(id);
            if (car == null)
                return NotFound();

            return NoContent();
        }   
    }
}
