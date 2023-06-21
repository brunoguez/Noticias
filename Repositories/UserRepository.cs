using Microsoft.Data.Sqlite;
using Noticias.Models;
using System.Data.SqlClient;

namespace Noticias.Repositories
{
    public class UserRepository
    {
        private static DataHelper helper = new();

        public User GetByEmailAndPassword(string email, string password)
        {
            List<User> users = helper.GetList<User>("select * from Usuario");

            users.Add(new User { Id = 14, Email = "Batman", Password = "batman", Perfil = "manager" });
            users.Add(new User { Id = 15, Email = "Robin", Password = "robin", Perfil = "employee" });
            return users.FirstOrDefault(x => x.Email.ToLower() == email.ToLower() && x.Password == password);
        }

        public List<User> GetUsers() => helper.GetList<User>("select * from Usuario");

        public int CreateUser(User user)
        {
            string cmd = @"insert into Usuario 
                (
                	email,
                	nome,
                	foto,
                	perfil,
                	senha
                )
                values
                (
                	@email,
                	@nome,
                	@foto,
                	@perfil,
                	@senha
                )";

            SqliteParameter[] sqlParameters =
            {
                new SqliteParameter("email", user.Email),
                new SqliteParameter("nome", user.Nome),
                new SqliteParameter("foto", user.Foto),
                new SqliteParameter("perfil", user.Perfil),
                new SqliteParameter("senha", user.Password)
            };

            return helper.ExecuteNonQuery(cmd, sqlParameters);
        }

        public User? GetByEmail(string email) => helper.GetList<User>("select * from Usuario").FirstOrDefault(x => x.Email.ToLower() == email.ToLower());

        public User? GetById(int id) => helper.GetList<User>("select * from Usuario").FirstOrDefault(x => x.Id == id);

        public void UpdateUser(User user)
        {
            string cmd = @"update Usuario 
                (
                	email,
                	nome,
                	foto,
                	perfil,
                	senha
                )
                set
                	email = @email,
                	nome = @nome,
                	foto = @foto,
                	perfil = @perfil,
                	senha = @senha";

            SqliteParameter[] sqlParameters =
            {
                new SqliteParameter("email", user.Email),
                new SqliteParameter("nome", user.Nome),
                new SqliteParameter("foto", user.Foto),
                new SqliteParameter("perfil", user.Perfil),
                new SqliteParameter("senha", user.Password)
            };

            helper.ExecuteNonQuery(cmd, sqlParameters);
        }

        internal void CreateUserCategoria(int id, int categoria)
        {
            string cmd = @"insert into CategoriaUsuario
                (
                    categoriaId, 
                    usuarioId
                )
                VALUES 
                (
                    @id, 
                    @categoria
                )";

            SqliteParameter[] sqlParameters =
            {
                new SqliteParameter("id", id),
                new SqliteParameter("categoria", categoria),
            };

            helper.ExecuteNonQuery(cmd, sqlParameters);
        }
    }
}
