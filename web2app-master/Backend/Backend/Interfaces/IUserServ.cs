using Backend.DTO;

namespace Backend.Interfaces
{
    public interface IUserServ
    {
        public Task<string> Login(UserLogDto loginUser);
        public Task<string> Register(UserRegDto newUser);
        public Task<UserGetDto> GetUserDetails(int id);
        public Task<string> Update(UserUpdateDto updatedUser);
        public Task<bool> Verify(int id, string action);
        public Task<List<UserSellersDto>> GetSellers();
    }
}
