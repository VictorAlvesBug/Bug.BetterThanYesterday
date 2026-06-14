using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Application.Users.DeleteUser;
using Bug.BetterThanYesterday.Application.Users.GetUserById;
using Bug.BetterThanYesterday.Application.Users.ListUsersByFilter;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Bug.BetterThanYesterday.Application.Users.UpdateUser;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[Route("testapi/[controller]")]
[ApiController]
public class UsersController(
	IUseCase<GetUserByIdCommand> getUserByIdUseCase,
	IUseCase<ListUsersByFilterCommand> listUsersByFilterUseCase,
	IUseCase<RegisterUserCommand> registerUserUseCase/*,
	IUseCase<UpdateUserCommand> updateUserUseCase,
	IUseCase<DeleteUserCommand> deleteUserUseCase*/)
	: ControllerBase
{
	[HttpGet("{userId}")]
	public async Task<IActionResult> GetById(Guid userId)
	{
		var command = new GetUserByIdCommand(userId);
		var result = await getUserByIdUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpGet]
	public async Task<IActionResult> ListByFilter([FromQuery] ListUsersByFilterCommand command)
	{
		var result = await listUsersByFilterUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Ok(result);

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	[HttpPost]
	public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
	{
		var result = await registerUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
		{
			var data = ((Result<UserModel>)result).Data;
			return Created($"Users/{data.Id}", result);
		}

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpPut]
	public async Task<IActionResult> Update([FromBody] UpdateUserCommand command)
	{
		var result = await updateUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return NoContent();

		if (result.IsRejected())
			return StatusCode(result.GetStatusCode(), result);

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
			return StatusCode(result.GetStatusCode(), result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}*/
}
