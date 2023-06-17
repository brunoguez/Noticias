using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Noticias.Models;
using Noticias.Services;
using Microsoft.IdentityModel.Tokens;

namespace Noticias.Controllers
{
    [Route("api/home")]
    public class HomeController : Controller
    {
        [HttpPost]
        [Route("auth")]
        [AllowAnonymous]
        public IActionResult Authenticate([FromQuery] User model)
        {
            var user = new UserService().GetByEmailAndPassword(model.Email, model.Password);
            if (user == null) return NotFound(new { massage = "Usuário ou senha inválidos" });

            var token = new TokenService().GenerateToken(user);
            user.Password = "";
            return Ok(new { user = user, token = token });
        }

        [HttpGet]
        [Route("authenticated")]
        [Authorize]
        public IActionResult Aunthenticated()
        {
            string? tokenOld = HttpContext.GetTokenAsync("access_token").Result;

            if (string.IsNullOrEmpty(tokenOld)) return BadRequest("Token Inválido");

            var tokenNew = new TokenService().RefreshToken(tokenOld);

            return Ok(new { massege = "Autenticado", token = tokenNew });
        }

        


    }
}
