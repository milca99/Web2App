﻿using AutoMapper;
using Backend.Database;
using Backend.DTO;
using Backend.Interfaces;
using Backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Services
{
    public class UserService : IUserServ
    {
        private readonly IConfiguration _configuration;
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IConfiguration configuration, IUserRepo userRepository, IMapper mapper)
        {
            _configuration = configuration;
            _userRepo = userRepository;
            _mapper = mapper;
        }

        public async Task<string> Login(UserLogDto loginUser)
        {
            var user = await _userRepo.Login(loginUser.Email, loginUser.Password);

            if (user == null)
                return null;

            var token = CreateJWT(user);
            return token;
        }

        private string CreateJWT(User user)
        {
            var secretKey = _configuration.GetSection("AppSettings:Key").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(secretKey!));

            var claims = new Claim[] {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Role,user.Role.ToString())
            };

            var signingCredentials = new SigningCredentials(
                    key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> Register(UserRegDto newUser)
        {
            byte[] passwordHash, passwordKey;

            using (var hmac = new HMACSHA512())
            {
                passwordKey = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(newUser.Password));

            }

            if (await _userRepo.DoesEmailExist(newUser.Email))
                return "emailexists";

            if (await _userRepo.DoesUsernameExist(newUser.Username))
                return "usernameexists";

            var user = _mapper.Map<User>(newUser);
            user.Password = passwordHash;
            user.PasswordKey = passwordKey;
            user.VerificationStatus = newUser.Role == "Customer" ? "Verified" : "Pending";
            user.Picture = newUser.Picture;
            var response = await _userRepo.Register(user);

            if (!response)
                return "failed";

            return "successful";
        }

        public async Task<UserGetDto> GetUserDetails(int id)
        {
            var result = await _userRepo.GetUserDetails(id);
            var returnValue = _mapper.Map<UserGetDto>(result);
            return returnValue;
        }

        public async Task<string> Update(UserUpdateDto updatedUser)
        {
            if (await _userRepo.DoesEmailExistExceptForThisUser(updatedUser.Email, updatedUser.Id))
                return "emailexists";

            if (await _userRepo.DoesUsernameExistExceptForThisUser(updatedUser.Username, updatedUser.Id))
                return "usernameexists";

            if (!await _userRepo.DoesUserExist(updatedUser.Id))
                return "nouserfound";

            if (String.IsNullOrWhiteSpace(updatedUser.Newpassword) && String.IsNullOrWhiteSpace(updatedUser.Oldpassword))
            {
                await _userRepo.Update(updatedUser);
            }
            else if (!String.IsNullOrWhiteSpace(updatedUser.Newpassword) && !String.IsNullOrWhiteSpace(updatedUser.Oldpassword))
            {
                if (await _userRepo.CheckOldPassword(updatedUser.Id, updatedUser.Oldpassword))
                    await _userRepo.Update(updatedUser);
                else
                    return "passwordError";
            }
            else
                return "passwordError";

            return "updated";
        }

        public async Task<bool> Verify(int id, string verificationStatus)
        {
            if (!await _userRepo.DoesSellerExist(id))
                return false;

            await _userRepo.Verify(id, verificationStatus);
            string message = verificationStatus == "Verified" ? "Congratulation you have been verified successfully!" : "Sorry, we currently have to much of sellers on our site... Better luck next time";
            return true;
        }
        public async Task<List<UserSellersDto>> GetSellers()
        {
            var results = await _userRepo.GetSellers();
            var returnValue = _mapper.Map<List<UserSellersDto>>(results);
            return returnValue;
        }
    }
}
