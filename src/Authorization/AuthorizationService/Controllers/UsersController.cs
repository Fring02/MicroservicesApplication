using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthorizationService.Dtos;
using AuthorizationService.Models;
using AuthorizationService.Repositories.Interfaces;
using AuthorizationService.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationService.Controllers
{
    [Route("api/v1/[controller]/")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _users;
        private readonly IMapper _mapper;
        private readonly JwtAuthentificationService<User> _authService;
        public UsersController(IUsersRepository users, IMapper mapper, JwtAuthentificationService<User> authService)
        {
            _users = users;
            _mapper = mapper;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserDto>> GetAllUsers()
        {
            var users = await _users.GetAll();
            if (users == null) return null;
            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        [HttpGet("{id}")]
        public async Task<UserDto> GetUserById(Guid id)
        {
            var user = await _users.GetById(id);
            if (user == null) return null;
            return _mapper.Map<UserDto>(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(Guid id)
        {
            var user = await _users.GetById(id);
            if (user == null) return BadRequest("This user doesn't exist");
            if (await _users.Delete(user)) return Ok("Deleted user");
            return StatusCode(500);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(UserDto updateUser, Guid id)
        {
            if (id == Guid.Empty) return BadRequest("Id required");
            var user = await _users.GetById(id);
            if (user == null) return BadRequest("This user doesn't exist");
            if (!string.IsNullOrEmpty(updateUser.Username)) user.Username = updateUser.Username;
            if (!string.IsNullOrEmpty(updateUser.Firstname)) user.Firstname = updateUser.Firstname;
            if (!string.IsNullOrEmpty(updateUser.Lastname)) user.Lastname = updateUser.Lastname;
            if (!string.IsNullOrEmpty(updateUser.Email)) user.Lastname = updateUser.Email;
            if (!string.IsNullOrEmpty(updateUser.Password))
            {
                EncryptionService.EncryptPassword(updateUser.Password, out byte[] hashed, out byte[] salt);
                user.HashedPassword = hashed;
                user.SaltPassword = salt;
            }
            if (await _users.Update(user)) return Ok("updated user");
            return StatusCode(500);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(RegistrationUserDto registerUser)
        {
            if (!ModelState.IsValid) return BadRequest("Fill all fields");
            var user = _mapper.Map<User>(registerUser);
            if (await _users.UserExists(user)) return BadRequest("This user already exists");
            user.Role = "user";
            user = await _users.Register(user, registerUser.Password);
            if (user == null) return StatusCode(500);
            var identity = _authService.CreateIdentity(user);
            return Ok(_authService.CreateToken(identity));
        }
        

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginUserDto loginUser)
        {
            if (!ModelState.IsValid) return BadRequest("Fill all fields");
            var user = _mapper.Map<User>(loginUser);
            if (!await _users.UserExists(user)) return BadRequest("This user doesn't exist");
            user = await _users.Login(user, loginUser.Password);
            if (user == null) return Unauthorized("Incorrect username or password");
            var identity = _authService.CreateIdentity(user);
            return Ok(_authService.CreateToken(identity));
        }
    }
}
