using Noticias.Models;
using Noticias.Repositories;

namespace Noticias.Services
{
    internal class NoticiasRepository
    {
        private static DataHelper helper = new();
        internal List<Noticia> GetNoticias()
        {
            return helper.GetList<Noticia>(@"SELECT n.*, u.nome autorNome, c.nome categoriaNome
                from Noticia n
                INNER JOIN Usuario u on u.idUsuario = n.autorId
                INNER JOIN Categoria c on c.idCategoria = n.categoriaId
                order by n.categoriaId");
        }
    }
}