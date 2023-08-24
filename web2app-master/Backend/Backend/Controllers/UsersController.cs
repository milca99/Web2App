using Backend.DTO;
using Backend.Interfaces;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserServ _userService;
        public UsersController(IUserServ userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLogDto loginUser)
        {
            var response = await _userService.Login(loginUser);

            if (response == null)
                return BadRequest("User doesn't exist");

            //return Ulogovanog korisnika da ga odma u store stavim
            return Ok(response);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserRegDto newUser)
        {
            if (newUser.Birthday.Date > DateTime.Now.Date)
                return BadRequest("Date is older than current date");

            var result = await _userService.Register(newUser);
            if (result == "failed")
                return BadRequest("Faild to register user to our shop");
            if (result == "emailexists")
                return BadRequest("Email already registered");
            if (result == "usernameexists")
                return BadRequest("Username already registered");
            return Ok();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer,Seller,Administrator")]
        public async Task<IActionResult> Details(int id)
        {
            if (id < 1)
                return BadRequest("Invalid id");
            var result = await _userService.GetUserDetails(id);
            if (result == null)
                return BadRequest("No user found");
            return Ok(result);
        }

        [HttpPatch("update")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Customer, Seller, Administrator")]
        public async Task<IActionResult> Update(UserUpdateDto updatedUser)
        {
            if (updatedUser.Birthday.Date > DateTime.Now.Date)
                return BadRequest("Date is older than current date");

            var response = await _userService.Update(updatedUser);

            if (response == "emailexists")
                return BadRequest("Email already exists");
            if (response == "usernameexists")
                return BadRequest("Username already exists");
            if (response == "nouserfound")
                return BadRequest("User not found");
            if (response == "passwordError")
                return BadRequest("Invalid new or old password");

            return Ok();
        }

        [HttpPatch("verify/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Administrator")]
        public async Task<IActionResult> Verify(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user id");
            if (!await _userService.Verify(id, "Verified"))
                return BadRequest("No users found with this id");
            return Ok();
        }

        [HttpPatch("deny/{id}")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Administrator")]
        public async Task<IActionResult> Deny(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid user id");
            if (!await _userService.Verify(id, "Denied"))
                return BadRequest("No users found with this id");
            return Ok();
        }

        [HttpGet("sellers")]
        [Authorize(Policy = "JwtSchemePolicy", Roles = "Administrator")]
        public async Task<IActionResult> GetSellers()
        {
            return Ok(await _userService.GetSellers());
        }
    }
}
