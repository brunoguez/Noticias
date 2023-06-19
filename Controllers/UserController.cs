using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Noticias.Models;
using Noticias.Services;
using System.Diagnostics;

namespace Noticias.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        public readonly UserService userService;
        private readonly IWebHostEnvironment _env;
        public UserController(IWebHostEnvironment env)
        {
            userService = new UserService();
            _env = env;
        }
        [HttpGet]
        public IActionResult GetUsers() => View();

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            return Ok();
        }

        [HttpPost]
        [Route("foto")]
        public IActionResult SaveFoto()
        {
            userService.SaveFoto(Request.Form, GetWwwRootPath());
            return Ok("Imagem recebida");
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateUser(User user)
        {
            try
            {
                User newUser = userService.CreateUser(user);
                return Ok(new { user = newUser });
            }
            catch (ExceptionService ex)
            {
                return StatusCode(ex.StatusCodeInt, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            try
            {
                string? tokenOld = HttpContext.GetTokenAsync("access_token").Result;

                if (string.IsNullOrEmpty(tokenOld)) return BadRequest("Token Inválido");

                TokenService tokenService = new();
                User? userVerify = tokenService.VerifyToken(tokenOld);

                if (userVerify is default(User)) return base.NotFound("Usuário não encontrado");
                if (id.Equals(userVerify.Id)) return BadRequest("Id difere do token");

                user.Id = id;
                UserService userService = new();
                userService.UpdateUser(userVerify);
                User? userUpdated = userService.GetById(user.Id);

                if (userUpdated is default(User)) return base.StatusCode(500, "Erro ao atualizar o usuário");

                var tokenRefresh = tokenService.RefreshToken(tokenOld);
                return Ok(new { user = userUpdated, token = tokenRefresh });
            }
            catch (ExceptionService ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id, User user)
        {
            return Ok();
        }
        public string GetWwwRootPath()
        {
            string wwwRootPath = _env.WebRootPath;
            return wwwRootPath;
        }
    }
}
