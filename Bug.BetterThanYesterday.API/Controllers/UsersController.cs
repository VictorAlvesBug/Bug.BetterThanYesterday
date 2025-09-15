using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Bug.BetterThanYesterday.Application.Users.RegisterUser;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(
	ListAllUsersUseCase listAllUsersUseCase,
	RegisterUserUseCase registerUserUseCase)
	: ControllerBase
{
	[HttpGet]
	public async Task<IActionResult> Get()
	{
		var result = await listAllUsersUseCase.HandleAsync(new ListAllUsersCommand());
		
		if (result.IsSuccess())
			return Ok(result);
		
		if(result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpGet("{id}")]
	public async Task<IActionResult> Get(string id)
	{
		return Ok(await _userRepository.GetByIdAsync(id));
	}*/

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] RegisterUserCommand command)
	{
		var result = await registerUserUseCase.HandleAsync(command);

		if (result.IsSuccess())
			return Created();

		if (result.IsRejected())
			return BadRequest(result);

		return StatusCode(StatusCodes.Status500InternalServerError, result);
	}

	/*[HttpPut]
	public async Task<IActionResult> Put([FromBody] User user)
	{
		await _userRepository.UpdateAsync(user);
		return NoContent();
	}*/

	/*[HttpDelete("{id}")]
	public async Task<IActionResult> Delete(string id)
	{
		var user = await _userRepository.GetByIdAsync(id);
		await _userRepository.DeleteAsync(user);
		return NoContent();
	}*/
}
