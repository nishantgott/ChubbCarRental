using ChubbCarRental.Models;
using System.Linq;

namespace ChubbCarRental.Repositories
{
    public class UserRepositary : IUserRepositary
    {
        private static List<UserModel> _users = new List<UserModel>();

        public void AddUser(UserModel user)
        {
            _users.Add(user);
        }

        public UserModel GetUserById(int id)
        {
            return _users.Find(x => x.Id == id);
        }

        public UserModel GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email == email);
        }
    }
}
