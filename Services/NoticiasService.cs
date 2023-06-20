
using Noticias.Models;

namespace Noticias.Services
{
    internal class NoticiasService
    {
        private readonly NoticiasRepository _repository;
        public NoticiasService() => _repository = new NoticiasRepository();

        internal Noticia CreatePublicacao(Noticia noticia)
        {
            _repository.CreatePublicacao(noticia);
            return _repository.GetNoticias()
                .Where(a => noticia.AutorId.Equals(a.AutorId))
                .OrderByDescending(a => a.Id)
                .First();
        }

        internal List<Categoria> GetCategorias()
        {
            return _repository.GetCategorias();
        }

        internal List<Noticia> GetNoticias()
        {
            return _repository.GetNoticias();
        }

        internal List<Noticia> GetNoticiasByUser(User user)
        {
            return _repository.GetNoticias()
                .Where(a => user.Id.Equals(a.AutorId))
                .OrderByDescending(a => a.DataPublicacao)
                .ToList();
        }

        internal void SaveImagemPublicacao(IFormCollection form, string local)
        {
            var file = form.Files[0];
            if (file.Length > 0)
            {
                var filePath = local + "\\img\\noticias\\" + ("undefined".Equals(form["id"]) ? file.FileName : $"{form["id"]}{Path.GetExtension(file.FileName)}");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyToAsync(stream);
                }
            }
        }

        internal void UpdatePublicacao(Noticia noticia)
        {
            _repository.UpdatePublicacao(noticia);
        }
    }
}