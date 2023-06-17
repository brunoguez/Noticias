using Noticias.Models;
using Noticias.Repositories;
using System.Net;

namespace Noticias.Services
{
    public class UserService
    {
        private readonly UserRepository _repository;
        public UserService() => _repository = new UserRepository();
        public List<User> GetUsers() => _repository.GetUsers();
        public User GetByEmailAndPassword(string email, string password) => _repository.GetByEmailAndPassword(email, password);
        public User CreateUser(User user)
        {
            User? newUser = _repository.GetByEmail(user.Email);
            if (newUser is not default(User)) throw new ExceptionService("Email já cadastrado", HttpStatusCode.Conflict);
            if (user.Email.Contains(' ') || !user.Email.Contains('@')) throw new ExceptionService("Email com caracteres inválidos", HttpStatusCode.BadRequest);
            _repository.CreateUser(user);
            newUser = _repository.GetByEmail(user.Email);
            newUser.Password = string.Empty;
            return newUser;
        }

        public void UpdateUser(User userUpdate)
        {
            User? user = _repository.GetById(userUpdate.Id);

            if (user is default(User)) throw new ExceptionService("Usuário não localizado ao realizar o update", HttpStatusCode.InternalServerError);

            userUpdate.GetType().GetProperties().ToList().ForEach(a =>  a.SetValue(user, userUpdate));

            _repository.UpdateUser(userUpdate);
        }

        public User? GetById(int id) => _repository.GetById(id);
    }
}
