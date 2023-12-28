using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sds.Events.Domain.Core;
using Sds.Events.Domain.Identity;
using Sds.Events.WebAPI.Dtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sds.Events.WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/user")]
    public class UserController : MainController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, UserManager<User> userManager, SignInManager<User> signInManager,
            IMapper mapper, INotifierMessage notifier)
        : base(notifier)
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
        [HttpGet("get-user")]
        public async Task<IActionResult> GetUser(string userName)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByNameAsync(userName);
                if (user == null)
                {
                    AddMessage("Usuário não encontrado");
                    return CustomResponse();
                }

                var userToReturn = _mapper.Map<UserDto>(user);
                userToReturn.PasswordHash = null;
                return CustomResponse(userToReturn);
            }
            catch (Exception e)
            {
                return HandleException($"Não foi possível obter usuário: {e.Message}");
            }
        }

        /// <summary>
        /// Login
        /// Não pode retornar BadRequest
        /// </summary>
        /// <param name="userLogin"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserLoginDto userLogin)
        {
            try
            {
                var user = await GetUserByUsername(userLogin.UserName);

                if (user == null || !await _userManager.CheckPasswordAsync(user, userLogin.PasswordHash))
                {
                    AddMessage("Não foi possível fazer login: usuário ou senha inválidos");
                    return CustomResponse(statusCode: 401);
                }

                var result = new
                {
                    token = await GenerateJwToken(user),
                    user = MapUser(user)
                };

                return CustomResponse(result);

            }
            catch (Exception e)
            {
                return HandleException($"Não foi possível fazer login: {e.Message}");
            }
        }

        /// <summary>
        /// Cria novo usuário
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return CustomResponse(ModelState);

                var user = _mapper.Map<User>(userDto);
                var result = await _userManager.CreateAsync(user, user.PasswordHash);

                if (!result.Succeeded)
                {
                    AddMessage("Não foi possível criar usuário");
                    AddMessageRange(result.Errors.Select(e => e.Description).ToArray());
                    return CustomResponse();
                }

                var userToResult = _mapper.Map<UserDto>(user);
                userToResult.PasswordHash = null;
                return CustomResponse(userToResult, 201, "user");
            }
            catch (Exception e)
            {
                return HandleException($"Não foi possível criar usuário: {e.Message}");
            }
        }

        #region private Methods

        /// <summary>
        /// Generate token
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task<string> GenerateJwToken(User user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["AppSettings:Token"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private UserLoginDto MapUser(User appUser)
        {
            var userDto = _mapper.Map<UserLoginDto>(appUser);
            userDto.PasswordHash = null;
            return userDto;
        }

        private async Task<User> GetUserByUsername(string username)
        {
            return await _userManager.Users
                .FirstOrDefaultAsync(u => u.NormalizedUserName == username.ToUpper());
        }
        #endregion private Methods
    }
}