using Microsoft.Data.Sqlite;
using Noticias.Models;
using Noticias.Repositories;
using System.Data.SqlClient;

namespace Noticias.Services
{
    internal class NoticiasRepository
    {
        private static DataHelper helper = new();

        internal void CreatePublicacao(Noticia noticia)
        {
            try
            {
                string cmd = @"insert into Noticia 
                (autorId, titulo, dataPublicacao, imagem, texto, publicada, categoriaId)
                values
                (@autorId, @titulo, (date('now')), @imagem, @texto, @publicada, @categoriaId)";

                SqliteParameter[] sqlParameters =
                {
                    new SqliteParameter("autorId", noticia.AutorId),
                    new SqliteParameter("titulo", noticia.Titulo),
                    new SqliteParameter("imagem", noticia.URL_imagem == null ? DBNull.Value : noticia.URL_imagem),
                    new SqliteParameter("texto", noticia.Texto),
                    new SqliteParameter("publicada", noticia.Publicada),
                    new SqliteParameter("categoriaId",noticia.CategoriaId)
                };

                helper.ExecuteNonQuery(cmd, sqlParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal List<Categoria> GetCategorias()
        {
            return helper.GetList<Categoria>(@"SELECT * from categoria");
        }

        internal List<Noticia> GetNoticias()
        {
            return helper.GetList<Noticia>(@"SELECT n.*, u.nome autorNome, c.nome categoriaNome
                from Noticia n
                INNER JOIN Usuario u on u.idUsuario = n.autorId
                INNER JOIN Categoria c on c.idCategoria = n.categoriaId
                order by n.categoriaId");
        }

        internal void UpdatePublicacao(Noticia noticia)
        {
            try
            {
                string cmd = @"update Noticia set
                    autorId = @autorId,
                    titulo = @titulo,
                    dataPublicacao = @dataPublicacao,
                    imagem = @imagem,
                    texto = @texto,
                    publicada = @publicada,
                    categoriaId =@categoriaId
                    where id = @id
                ";

                SqliteParameter[] sqlParameters =
                {
                    new SqliteParameter("autorId", noticia.AutorId),
                    new SqliteParameter("titulo", noticia.Titulo),
                    new SqliteParameter("dataPublicacao", noticia.DataPublicacao),
                    new SqliteParameter("imagem", noticia.URL_imagem),
                    new SqliteParameter("texto", noticia.Texto),
                    new SqliteParameter("publicada", noticia.Publicada),
                    new SqliteParameter("categoriaId",noticia.CategoriaId),
                    new SqliteParameter("id",noticia.Id),
                };

                helper.ExecuteNonQuery(cmd, sqlParameters);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}