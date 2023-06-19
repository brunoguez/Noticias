
using Noticias.Models;

namespace Noticias.Services
{
    internal class NoticiasService
    {
        private readonly NoticiasRepository _repository;
        public NoticiasService() => _repository = new NoticiasRepository();

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
    }
}