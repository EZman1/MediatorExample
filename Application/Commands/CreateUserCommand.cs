using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using FluentValidation;
using MediatR;

namespace Application.Commands;

public class CreateUserCommand : IRequest
{
    public UserDto CreateUser { get; }

    public CreateUserCommand(UserDto createUser)
    {
        CreateUser = createUser;
    }
}

internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CreateUser.Name).NotEmpty();
        RuleFor(x => x.CreateUser.EmailAddress).NotEmpty();
    }
}

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUserRepository _userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _userRepository.CreateUser(new User { Name = request.CreateUser.Name, EmailAddress = request.CreateUser.EmailAddress });
        return Task.CompletedTask;
    }
}
