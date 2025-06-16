using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mind_Your_Drink_Models.Data;
using Mind_Your_Drink_Models.DTO_s;
using Mind_Your_Drink_Models.Models;
using Mind_Your_Drink_Models.Utilities;

namespace Mind_Your_Drink_Models.Controllers
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
        public async Task<IActionResult> CreateUserDrink([FromBody] UserDrinkRequest request)
        {
            var user = await _unitOfWork.Users.GetByName(request.Name);
            if (user == null) 
                return NotFound("User is not Found");
            if (user.HashPassword != request.Hash)
                return BadRequest("Password is not correct");
            Console.WriteLine($"Date being saved: {request.UserDrink.Time}");

            request.UserDrink.UserId = user.Id;

            _unitOfWork.UserDrinks.Add(request.UserDrink);
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpPost("GetAllDrinks")]
        public async Task<IActionResult> GetAllDrinks ([FromBody] UserNameRequest request)
        {
            var user = await _unitOfWork.Users.GetByName(request.Name);

            if (user == null)
            {
                return NotFound($"User with name '{request.Name}' not found.");
            }

            var Drinks = await _unitOfWork.UserDrinks.GetAllByUserId(user.Id);

            return Ok(Drinks);
        }

        [HttpPost("GetDrinksByDay")]
        public async Task<IActionResult> GetDrinksByDay([FromBody] DrinkByDayRequest request)
        {
            var user = await _unitOfWork.Users.GetByName(request.Name);

            if (user == null)
            {
                return NotFound($"User with name '{request.Name}' not found.");
            }

            var Drinks = await _unitOfWork.UserDrinks.GetByDayByUserIdAsync(user.Id, request.Date);

            return Ok(Drinks);
        }

        [HttpPost("GetDrinksByPeriod")]
        public async Task<IActionResult> GetDrinksByPeriod([FromBody] PeriodRequest request)
        {

            var user = await _unitOfWork.Users.GetByName(request.Name);
            if (user == null)
                return NotFound("User is not Found");

            try
            {
                var drinks = await _unitOfWork.UserDrinks.GetDrinksByPeriodAsync(
                    user.Id,
                    request.Period
                );
                return Ok(drinks);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
