using ChubbCarRental.Models;
namespace ChubbCarRental.Repositories
{
    public interface IUserRepositary
    {
        public void AddUser(UserModel user);
        public UserModel GetUserById(int id);
        public UserModel GetUserByEmail(string email);
    }
}