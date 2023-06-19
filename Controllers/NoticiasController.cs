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
        [Route("Publicar")]
        public IActionResult Publicar()
        {
            return View();
        }

        [HttpGet]
        [Route("api/GetNoticias")]
        public IActionResult GetNoticias()
        {
            NoticiasService noticiasService = new();
            List<Noticia> noticiasList = noticiasService.GetNoticias();
            List<string> categoriaList = noticiasList.DistinctBy(a => a.CategoriaNome).Select(a => a.CategoriaNome).ToList();

            //Dictionary<string, List<Noticia>> noticias = new();
            List<object> noticias = new();
            categoriaList.ForEach(a =>
            {
                noticias.Add(new { categoria = a, noticiasList = noticiasList.Where(b => b.CategoriaNome.Equals(a) && b.Publicada).OrderByDescending(a => a.DataPublicacao).ToList() });
            });
            return Ok(new { noticias, categoriaList });
        }

        [HttpGet]
        [Route("api/GetPublicacao")]
        public IActionResult GetPublicacao()
        {
            NoticiasService noticiasService = new();

            User user = new() { Id = 1 };

            List<Categoria> categorias = noticiasService.GetCategorias();

            List<object> noticias = new();
            noticiasService.GetNoticiasByUser(user).ForEach(a => noticias.Add(new { a, desc = $"{a.Id} - {a.Titulo} | {a.DataPublicacao:dd/MM/yyy}" }));

            return Ok(new { noticias, categorias });
        }

    }

}
