
using Noticias.Models;

namespace Noticias.Services
{
    internal class NoticiasService
    {
        private readonly NoticiasRepository _repository;
        public NoticiasService() => _repository = new NoticiasRepository();
        internal List<Noticia> GetNoticias()
        {
            return _repository.GetNoticias();
        }
    }
}