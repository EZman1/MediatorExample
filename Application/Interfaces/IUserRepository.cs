using Domain.Entities;

namespace Application.Interfaces;

public interface IUserRepository
{
    User GetUser(int id);
    List<User> GetUsers();
    void CreateUser(User user);
    void DeleteUser(int id);
}