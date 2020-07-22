using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Proagil.WebAPI.Dtos;
using ProAgil.Domain.Identity;

namespace Proagil.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtém usuário
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpGet("user")]
        public async Task<IActionResult> Get(UserDto userDto)
        {
            return Ok(userDto);
        }

        /// <summary>
        /// Cria novo usuário
        /// Não necessita de autenticação [AllowAnonymous]
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, user.PasswordHash);
                var userToResult = _mapper.Map<UserDto>(user);

                if (result.Succeeded)
                    return Created("user", user);

                return BadRequest(result.Errors);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Não foi possível criar usuário: {e.Message}");
            }
        }
    }
}