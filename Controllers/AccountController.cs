using EaTSWebAPI.Data;
using EaTSWebAPI.Models;
using EaTSWebAPI.Models.Users;
using EaTSWebAPI.Service;
using EaTSWebAPI.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static EaTSWebAPI.WC;

namespace EaTSWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }
        private readonly ApplicationDbContext _db;


        /// <summary>
        /// Получить токен
        /// </summary>
        /// <param name="request">Логин и пароль</param>
        /// <returns>Возвращает токен и данные пользователя</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     {
        ///         "username": "admin",
        ///         "password": "admin"
        ///     }
        ///
        /// </remarks>
        /// <response code="400">Не верный логин или пароль</response>
        [Route("GetToken")]
        [HttpPost]
        public IActionResult Token(LoginRequest request)
        {
            var identityResponse = GetIdentity(request.Username, request.Password);
            if (identityResponse == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identityResponse.claimsIdentity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                accessToken = encodedJwt,
                user = identityResponse.user
            };

            return new ObjectResult(response);
        }

        /// <summary>
        /// Проверка авторизации
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Авторизация успешна</response>
        /// <response code="401">Авторизация отсутствует</response>
        [Authorize]
        [Route("AuthorizationCheck")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult AuthorizationCheck()
        {
            return Ok("Ура! Вы авторизированы!");
        }

        private IdentityResponse? GetIdentity(string username, string password)
        {
            User user = _db.User.Include(a => a.Agency).FirstOrDefault(u => u.Login == username && u.Password == password);

            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString())
                };

            ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            IdentityResponse IdentityResponse = new()
            {
                claimsIdentity = claimsIdentity,
                user = user
            };
            return IdentityResponse;


        }
    }
}
