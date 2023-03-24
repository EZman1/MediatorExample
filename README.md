# Mediator Pattern in C# with CQRS
The mediator pattern is a behavioral design pattern that allows objects to communicate with each other without being tightly coupled. In the context of CQRS (Command Query Responsibility Segregation), the mediator pattern is commonly used to handle communication between requests and their respective handlers.
## What is CQRS?
CQRS is an architectural pattern that separates the read and write operations of an application into separate paths. This separation allows for better scalability, performance, and maintainability of the application. In a CQRS architecture, **commands** are responsible for modifying the state of the system, while **queries** are responsible for retrieving data from the system.
## What is the Mediator Pattern?
The mediator pattern is a behavioral design pattern that promotes loose coupling between objects. It involves creating a mediator object that encapsulates the communication between a set of objects. Each object communicates with the mediator rather than with other objects directly. This allows for greater flexibility and decoupling between objects.
In simple terms, the mediator pattern establishes a communication channel between objects and promotes encapsulation of interactions between objects. Instead of direct communication between objects, they communicate through a mediator object. This allows for decoupling of objects and improves maintainability of the codebase.
## Mediator Pattern with CQRS
In the context of CQRS, the mediator pattern is commonly used to handle communication between requests and their respective handlers. The mediator is responsible for receiving requests, identifying the appropriate handler for the command, and invoking the handler. The handler then performs the required action and returns the result to the mediator.
The mediator pattern is particularly useful in CQRS architectures because it promotes loose coupling between requests and their respective handlers. This allows for greater flexibility in handling requests and makes it easier to add new requests to the system.
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
This code shows an implementation of a Query using the MediatR library. In this code, there are two classes: **GetUserQuery** and **GetUserQueryHandler**.

**GetUserQuery** is a simple data class that represents the request to retrieve a user. It has a single property Id of type int, which represents the ID of the user to retrieve.

**GetUserQueryHandler** is the class that handles the GetUserQuery request. The Handle method takes the **GetUserQuery** request and it is responsible for executing the request and returning the data.

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
This code shows an implementation of a Command using the MediatR library. In this code, there are two classes: **CreateUserCommand** and **CreateUserCommandHandler**.

**CreateUserCommand** is a simple data class that represents the request to create a user.

**CreateUserCommandHandler** is the class that handles the **CreateUserCommand** request. The Handle method takes the **CreateUserCommand** request and it is responsible for executing the request.

## Validation

Each handler can have it's own validation, this can be done with a pipeline behaviour. For this validation we use FluentValidation.
```
internal class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.CreateUser.Name).NotEmpty();
        RuleFor(x => x.CreateUser.EmailAddress).NotEmpty();
    }
}
```
The **CreateUserCommandValidator** is a class that derives from the **AbstractValidator<T>** class in the FluentValidation library, and it defines rules for validating a **CreateUserCommand** object.
In this example, the validator defines two rules using the **RuleFor** method. Each rule specifies a property of the **CreateUserCommand** object that should be validated, and the validation logic to apply. The rules validate that the **Name** and **EmailAddress** properties of the **CreateUser** object are not empty. This ensures that the user's name and email address is provided when creating a new user. If either property is empty, a validation error is added to the validation result.

```
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
{
    private readonly IEnumerable<IValidator<TRequest>> validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        this.validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (validators.Any())
        {
            foreach (var validator in validators)
            {
                var result = await validator.ValidateAsync(request);
                HandleValidationResult(request, result);
            }
        }

        return await next();
    }

    private static void HandleValidationResult(TRequest request, ValidationResult result)
    {
        if (!result.IsValid)
        {
            throw new ValidationException(request, result.Errors, result.RuleSetsExecuted);
        }
    }
}
```
This is an example of a validation behavior class. In this case it will validate all requests with the **IRequest** interface.

## Registering Dependencies
```
public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));
        
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
```
The **AddApplicationServices** method is an extension method for the **IServiceCollection** interface, which is used to register the services required for the application.
This registers the MediatR library and sets up MediatR to automatically scan for and register command and query handlers in the assembly containing the **ConfigureServices** class.

```
services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ConfigureServices).Assembly));
```
This scans all assemblies in the current domain for classes that derive from the **AbstractValidator<T>** class in the **FluentValidation** library, and registers them with the dependency injection container.

```
services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies(), includeInternalTypes: true);
```
This registers validators from all assemblies in the current application domain. This ensures that all validators are registered in the dependency injection container and available for use when validating requests.

The includeInternalTypes parameter specifies whether to include internal types in the assemblies. If this parameter is set to true, internal validators will also be registered in the container.

```
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
```
This registers the **ValidationBehavior** class as a scoped pipeline behavior with the dependency injection container. The **typeof(IPipelineBehavior<,>)** parameter specifies the interface that the behavior implements, and the typeof**(ValidationBehavior<,>)** parameter specifies the behavior class.

## Sending a Request
```
[HttpGet("{id}")]
public async Task<IActionResult> GetUser(int id)
{
    return Ok(await _mediator.Send(new GetUserQuery(id)));
}
```
Here is an example on how you can get the user with Mediator. The **await _mediator.Send(new GetUserQuery(id))** statement sends the **GetUserQuery** object to the Mediator, which handles the request by invoking the **GetUserQueryHandler** class to retrieve the user from a data store.

## Conclusion
Using the Mediator Pattern in C# with CQRS promotes loose coupling between components, separates concerns, and improves maintainability, testability, and scalability. It achieves this by decoupling command and query handlers from the application logic and providing a single point of entry for sending commands and queries to their respective handlers, allowing for the addition of cross-cutting concerns to the Mediator pipeline.

## Code
https://github.com/EZman1/MediatorExample

In this project we used the Clean Architecture as a template to setup our code (see https://github.com/jasontaylordev/CleanArchitecture) 

## Additional links
* https://github.com/jbogard/MediatR
* https://www.youtube.com/watch?v=YzOBrVlthMk
* https://refactoring.guru/design-patterns/mediator
* https://code-maze.com/cqrs-mediatr-in-aspnet-core/
* https://github.com/jasontaylordev/CleanArchitecture
