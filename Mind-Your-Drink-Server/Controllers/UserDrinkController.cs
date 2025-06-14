using Microsoft.AspNetCore.Mvc;
using Mind_Your_Drink_Models.Utilities;
using Mind_Your_Drink_Server.Data;
using Mind_Your_Drink_Server.DTO_s;
using Mind_Your_Drink_Server.Models;

namespace Mind_Your_Drink_Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDrinkController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserDrinkController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("CreateUserDrink")]
        public async Task<IActionResult> Ban([FromBody] UserDrinkRequest request)
        {
            var user = await _unitOfWork.Users.GetByName(request.Name);
            if (user == null) 
                return NotFound("User is not Found");
            if (user.HashPassword != request.Hash)
                return BadRequest("Password is not correct");

            request.UserDrink.UserId = user.Id;

            _unitOfWork.UserDrinks.Add(request.UserDrink);
            _unitOfWork.Complete();

            return Ok();
        }

    }
}
