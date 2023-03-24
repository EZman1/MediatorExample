using Application.Interfaces;
using MediatR;

namespace Application.Commands;

public class DeleteUserCommand : IRequest
{
    public int Id { get; }

    public DeleteUserCommand(int id)
    {
        Id = id;
    }
}

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _userRepository.DeleteUser(request.Id);
        return Task.CompletedTask;
    }
}
