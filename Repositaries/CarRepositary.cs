using ChubbCarRental.Data;
using ChubbCarRental.Model;
using Microsoft.EntityFrameworkCore;

namespace ChubbCarRental.Repositories
{
    public class CarRepositary : ICarRepositary
    {
        private readonly CarRentalDbContext _context;

        public CarRepositary(CarRentalDbContext context)
        {
            _context = context;
        }

        public void AddCar(CarModel car)
        {
            _context.Cars.Add(car);
            _context.SaveChanges();
        }

        public CarModel GetCarById(int id)
        {
            return _context.Cars.FirstOrDefault(x => x.Id == id);
        }

        public List<CarModel> GetAvailableCars()
        {
            return _context.Cars.ToList();
        }

        public void UpdateCarAvailability(int id, bool isAvailable)
        {
            var car = _context.Cars.FirstOrDefault(x => x.Id == id);
            if (car != null)
            {
                car.IsAvailable = isAvailable;
                _context.SaveChanges();
            }
        }
    }
}
