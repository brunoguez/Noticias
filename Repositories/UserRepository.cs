using Noticias.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;

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

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("email", user.Email),
                new SqlParameter("nome", user.Nome),
                new SqlParameter("foto", user.Foto),
                new SqlParameter("perfil", user.Perfil),
                new SqlParameter("senha", user.Password)
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

            SqlParameter[] sqlParameters =
            {
                new SqlParameter("email", user.Email),
                new SqlParameter("nome", user.Nome),
                new SqlParameter("foto", user.Foto),
                new SqlParameter("perfil", user.Perfil),
                new SqlParameter("senha", user.Password)
            };

            helper.ExecuteNonQuery(cmd, sqlParameters);
        }

    }
}
