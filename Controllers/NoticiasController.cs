using Microsoft.AspNetCore.Mvc;
using Noticias.Models;
using Noticias.Services;

namespace Noticias.Controllers
{
    public class NoticiasController : Controller
    {
        [HttpGet("noticias")]
        public IActionResult Noticias()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetNoticias()
        {
            NoticiasService noticiasService = new();
            List<Noticia> noticias = noticiasService.GetNoticias();
        }
    }
}
