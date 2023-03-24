# Mediator Pattern in C# with CQRS
The mediator pattern is a behavioral design pattern that allows objects to communicate with each other without being tightly coupled. In the context of CQRS (Command Query Responsibility Segregation), the mediator pattern is commonly used to handle communication between commands and their respective handlers.
## What is CQRS?
CQRS is an architectural pattern that separates the read and write operations of an application into separate paths. This separation allows for better scalability, performance, and maintainability of the application. In a CQRS architecture, commands are responsible for modifying the state of the system, while queries are responsible for retrieving data from the system.
## What is the Mediator Pattern?
The mediator pattern is a behavioral design pattern that promotes loose coupling between objects. It involves creating a mediator object that encapsulates the communication between a set of objects. Each object communicates with the mediator rather than with other objects directly. This allows for greater flexibility and decoupling between objects.
In simple terms, the mediator pattern establishes a communication channel between objects and promotes encapsulation of interactions between objects. Instead of direct communication between objects, they communicate through a mediator object. This allows for decoupling of objects and improves maintainability of the codebase.
## Mediator Pattern with CQRS
In the context of CQRS, the mediator pattern is commonly used to handle communication between commands and their respective handlers. The mediator is responsible for receiving commands, identifying the appropriate handler for the command, and invoking the handler. The handler then performs the required action and returns the result to the mediator.
The mediator pattern is particularly useful in CQRS architectures because it promotes loose coupling between commands and their respective handlers. This allows for greater flexibility in handling commands and makes it easier to add new commands to the system.
The following is an example implementation of the mediator pattern in C# with CQRS:

## Query example

```
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

```
This code shows an implementation of a Query using the MediatR library. In this code, there are two classes: GetUserQuery and GetUserQueryHandler.

GetUserQuery is a simple data class that represents the request to retrieve a user. It has a single property Id of type int, which represents the ID of the user to retrieve.

GetUserQueryHandler is the class that handles the GetUserQuery request. The Handle method takes the GetUserQuery request and it is responsible for executing the request and returning the data.

## Command example

```
public class CreateUserCommand : IRequest
{
    public UserDto CreateUser { get; }

    public CreateUserCommand(UserDto createUser)
    {
        CreateUser = createUser;
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

```
This code shows an implementation of a Command using the MediatR library. In this code, there are two classes: CreateUserCommand and CreateUserCommandHandler.

CreateUserCommand is a simple data class that represents the request to create a user.

CreateUserCommandHandler is the class that handles the CreateUserCommand request. The Handle method takes the CreateUserCommand request and it is responsible for executing the request.

## Conclusion
The mediator pattern is a useful pattern for promoting loose coupling between objects. In the context of CQRS, it is commonly used to handle communication between commands and their respective handlers. By using the mediator pattern, you can create a more flexible and maintainable architecture for your CQRS applications.
By implementing the mediator pattern, you can improve the scalability, performance, and maintainability of your CQRS applications. The mediator pattern allows for loose coupling between objects and promotes encapsulation of interactions between objects. This makes it easier to add new commands to the system and maintain the codebase over time.

## Additional links
* https://github.com/jbogard/MediatR
* https://www.youtube.com/watch?v=YzOBrVlthMk
* https://refactoring.guru/design-patterns/mediator
* https://code-maze.com/cqrs-mediatr-in-aspnet-core/
