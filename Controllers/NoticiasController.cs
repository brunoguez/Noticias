using Microsoft.AspNetCore.Mvc;
using Noticias.Models;
using Noticias.Services;

namespace Noticias.Controllers
{
    [Route("Noticias")]
    public class NoticiasController : Controller
    {
        [HttpGet]
        public IActionResult Noticias()
        {
            return View();
        }

        [HttpGet]
        [Route("api/GetNoticias")]
        public IActionResult GetNoticias()
        {
            NoticiasService noticiasService = new();
            List<Noticia> noticias = noticiasService.GetNoticias();
            List<string> categoriaList = noticias.DistinctBy(a => a.CategoriaNome).Select(a => a.CategoriaNome).ToList();
            return Ok(new { noticias, categoriaList });
        }
    }
}
