using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.DeleteUser;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
	IUseCase<ListAllUsersCommand> listAllUsersUseCase,
	IUseCase<GetUserByIdCommand> getUserByIdUseCase,
	IUseCase<RegisterUserCommand> registerUserUseCase,
	IUseCase<UpdateUserCommand> updateUserUseCase,
	IUseCase<DeleteUserCommand> deleteUserUseCase)
	: ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> ListAll()
	{
		var command = new ListAllUsersCommand();
		var result = await listAllUsersUseCase.HandleAsync(command);
		
		if (result.IsSuccess())
			return Ok(result);
		
		if(result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet("{userId}")]
	public async Task<IActionResult> GetById(Guid userId)
	{
		var command = new GetUserByIdCommand(userId);
		var result = await getUserByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
	{
		var result = await registerUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<UserModel>)result).Data;
			return Created($"Users/{data.UserId}", result);
		}

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
	{
		var result = await updateUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpDelete("{userId}")]
	public async Task<IActionResult> Delete(Guid userId)
	{
		var command = new DeleteUserCommand(userId);
		var result = await deleteUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}
}
