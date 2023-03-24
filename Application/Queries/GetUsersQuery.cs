using Application.Interfaces;
using Application.Models;
using MediatR;

namespace Application.Queries;

public class GetUsersQuery : IRequest<List<UserDto>>
{
}

public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return Task.FromResult(_userRepository.GetUsers()
            .Select(x => new UserDto() { Name = x.Name, EmailAddress = x.EmailAddress }).ToList());
    }
}