using Application.Interfaces;
using Application.Models;
using FluentValidation;
using MediatR;

namespace Application.Queries;

public class GetUserQuery : IRequest<UserDto>
{
    public int Id { get; }

    public GetUserQuery(int id)
    {
        Id = id;
    }
}

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetUser(request.Id);
        var userDto = new UserDto { Name = user.Name, EmailAddress = user.EmailAddress };
        return Task.FromResult(userDto);
    }
}
