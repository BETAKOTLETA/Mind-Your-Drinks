using Microsoft.AspNetCore.Mvc;
using Mind_Your_Drink_Models.Utilities;
using Mind_Your_Drink_Server.Data;
using Mind_Your_Drink_Server.DTO_s;
using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
                return Conflict("Name or Password is null");

            if (await _unitOfWork.Users.IsExist(request.Name))
                return Unauthorized("Account with this UserName already exist");

            User NewUser = Models.User.CreateUser(request.Name, request.Password);

            _unitOfWork.Users.Add(NewUser);
            _unitOfWork.Complete();

            return Ok("Account is Created");
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
                return Conflict("Name or Password is null");

            if (! await _unitOfWork.Users.IsExist(request.Name))
                return NotFound("Account Not Found");
                
            User User = await _unitOfWork.Users.GetByName(request.Name);

            if(User.HashPassword != request.Password.ToHashPassword())
                return Unauthorized("Password is not correct");

            return Ok("Login");
        }

    }
}
