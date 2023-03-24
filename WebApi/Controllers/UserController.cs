using Application.Commands;
using Application.Exceptions;
using Application.Models;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController  : ControllerBase
{
    private readonly IMediator _mediator;
    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        return Ok(await _mediator.Send(new GetUserQuery(id)));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _mediator.Send(new GetUsersQuery()));
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserDto createUser)
    {
        try
        {
            await _mediator.Send(new CreateUserCommand(createUser));
            return Ok();
        }
        catch (ValidationException ex)
        {
            return BadRequest(ex.Errors);
        }
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _mediator.Send(new DeleteUserCommand(id));
        return Ok();
    }
}