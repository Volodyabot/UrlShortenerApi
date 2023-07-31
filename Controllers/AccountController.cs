using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using UrlShortener.DTOs;
using UrlShortener.Interfaces;
using UrlShortener.Models;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterUserDto registerUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userRepository.RegisterAsync(registerUserDto);

                if (result.Token != "")
                {
                    return Ok(result);
                }

                return BadRequest("Not registered");
            }
            catch (System.Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] LoginUserDto loginUserDto)
        {
            try
            {
                LoginUserResponseDto result = await _userRepository.LoginAsync(loginUserDto);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("AssignRole")]
        public async Task<ActionResult> AssignRole(string userName, string role)
        {
            var result = await _userRepository.AssignRole(userName, role);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("User not found");
        }
    }
}