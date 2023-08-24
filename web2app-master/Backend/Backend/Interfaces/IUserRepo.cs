using Backend.DTO;
using Backend.Models;

namespace Backend.Interfaces
{
    public interface IUserRepo
    {
        public Task<User> Login(string email, string password);
        public Task<bool> DoesEmailExist(string email);
        public Task<bool> DoesUsernameExist(string username);
        public Task<bool> Register(User newUser);
        public Task<User> GetUserDetails(int id);
        public Task<bool> CheckOldPassword(int id, string password);
        public Task<User> Update(UserUpdateDto updatedUser);
        public Task<User> Verify(int userId, string verificationStatus);
        public Task<List<User>> GetSellers();
        public Task<bool> DoesUserExist(int id);
        public Task<bool> DoesSellerExist(int id);
        public Task<bool> DoesEmailExistExceptForThisUser(string email, int id);
        public Task<bool> DoesUsernameExistExceptForThisUser(string username, int id);
    }
}
