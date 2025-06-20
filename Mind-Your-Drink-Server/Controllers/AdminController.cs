﻿using Microsoft.AspNetCore.Mvc;
using Mind_Your_Drink_Models.Utilities;
using Mind_Your_Drink_Models.Data;
using Mind_Your_Drink_Models.DTO_s;
using Mind_Your_Drink_Models.Models;

namespace Mind_Your_Drink_Models.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpPost("Ban")]
        public async Task<IActionResult> Ban([FromBody] ToBanRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.ToBanName) || string.IsNullOrWhiteSpace(request.AdminPassword))
                return Conflict("Name or Password is null");

            var user = await _unitOfWork.Users.GetByName(request.ToBanName);
            user.Initialize();

            Console.WriteLine(user.StateName);

            var admin = await _unitOfWork.Admins.GetByHash(request.AdminPassword);

            admin.Ban(user);

            Console.WriteLine(user.StateName);

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();


            return Ok();
        }

        [HttpPost("UnBan")]
        public async Task<IActionResult> UnBan([FromBody] ToBanRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.ToBanName) || string.IsNullOrWhiteSpace(request.AdminPassword))
                return Conflict("Name or Password is null");

            var user = await _unitOfWork.Users.GetByName(request.ToBanName);
            user.Initialize();

            Console.WriteLine(user.StateName);

            var admin = await _unitOfWork.Admins.GetByHash(request.AdminPassword);

            admin.UnBan(user);

            Console.WriteLine(user.StateName);

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();


            return Ok();
        }

        [HttpPost("CreateAdmin")]
        public async Task<IActionResult> Create([FromBody] UserCreateRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
                return Conflict("Name or Password is null");

            if (await _unitOfWork.Admins.IsExist(request.Name))
                return Unauthorized("Account with this UserName already exist");

            Admin NewAdmin = Admin.CreateAdmin(request.Name, request.Password);

            _unitOfWork.Admins.Add(NewAdmin);
            _unitOfWork.Complete();

            return Ok("Account is Created");
        }

        [HttpPost("GetAllUserInfo")]
        public async Task<IActionResult> GetAllUserInfo([FromBody] UserCreateRequest request)
        {
            if (request is null)
                return BadRequest("Request is null");

            if (string.IsNullOrWhiteSpace(request.Name) || string.IsNullOrWhiteSpace(request.Password))
                return Conflict("Name or Password is null");

            if (!(await _unitOfWork.Admins.IsExist(request.Name)) )
                return Unauthorized("Account with this UserName not exist");

            Admin Admin = await _unitOfWork.Admins.GetByName(request.Name);

            if(Admin.HashPassword != request.Password)
                return Unauthorized(Admin.HashPassword + " " + request.Password);

            var Users = await _unitOfWork.Users.GetAllUsers(); 

            return Ok(Users);
        }

    }
}
