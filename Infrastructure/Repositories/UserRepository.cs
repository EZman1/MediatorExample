using Application.Interfaces;
using Domain.Entities;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly Dictionary<int, User> _users;
    
    public UserRepository()
    {
        _users = new Dictionary<int, User>
        {
            {1, new User {Id = 1, Name = "John Doe", EmailAddress = "john.doe@mail.com"}},
            {2, new User {Id = 2, Name = "Jane Doe", EmailAddress = "jane.doe@mail.com"}},
        };
    }
    public User GetUser(int id)
    {
        return _users[id];
    }

    public List<User> GetUsers()
    {
        return _users.Values.ToList();
    }

    public void CreateUser(User user)
    {
        var newId = _users.Max(x => x.Key) + 1;
        user.Id = newId;
        _users.Add(user.Id, user);
    }

    public void DeleteUser(int id)
    {
        _users.Remove(id);
    }
}