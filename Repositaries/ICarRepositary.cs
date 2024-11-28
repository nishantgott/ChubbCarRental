using ChubbCarRental.Model;

namespace ChubbCarRental.Repositories
{
    public interface ICarRepositary
    {
        public void AddCar(CarModel car);
        public CarModel GetCarById(int id);
        public List<CarModel> GetAvailableCars();
        public void UpdateCarAvailability(int id, bool isAvailable);
    }   
}