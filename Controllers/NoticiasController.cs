using Microsoft.AspNetCore.Mvc;
using Noticias.Models;
using Noticias.Services;

namespace Noticias.Controllers
{
    [Route("Noticias")]
    public class NoticiasController : Controller
    {
        private readonly IWebHostEnvironment _env;
        public NoticiasController(IWebHostEnvironment env) =>_env = env;
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

        [HttpPost]
        [Route("api/CreatePublicacao")]
        public IActionResult CreatePublicacao(Noticia noticia)
        {
            try
            {
                NoticiasService noticiasService = new();
                Noticia newNoticia = noticiasService.CreatePublicacao(noticia);
                return Ok(newNoticia);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut]
        [Route("api/UpdatePublicacao/{id}")]
        public IActionResult UpdatePublicacao([FromRoute]int id, Noticia noticia)
        {
            try
            {
                NoticiasService noticiasService = new();
                noticia.Id = id;
                noticiasService.UpdatePublicacao(noticia);
                return Ok("Noticia atualizada");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("api/SaveImagemPublicacao")]
        public IActionResult SaveImagemPublicacao()
        {
            NoticiasService noticiasService = new();
            noticiasService.SaveImagemPublicacao(Request.Form, GetWwwRootPath());
            return Ok("Imagem recebida");
        }

        [HttpGet]
        [Route("api/GetPublicacao")]
        public IActionResult GetPublicacao()
        {
            NoticiasService noticiasService = new();

            UserService userService = new();
            User user = userService.GetById(2);
            user.Password = string.Empty;

            List<Categoria> categorias = noticiasService.GetCategorias();

            List<object> noticias = new();
            noticiasService.GetNoticiasByUser(user).ForEach(notice => noticias.Add(new { notice, id = notice.Id, desc = $"{notice.Id} - {notice.Titulo} | {notice.DataPublicacao:dd/MM/yyy}" }));



            return Ok(new { noticias, categorias, user});
        }
        public string GetWwwRootPath()
        {
            string wwwRootPath = _env.WebRootPath;
            return wwwRootPath;
        }

    }

}
