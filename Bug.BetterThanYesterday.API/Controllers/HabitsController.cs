using Bug.BetterThanYesterday.Application.Habits.CreateHabit;
using Bug.BetterThanYesterday.Application.Users.ListAllUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class HabitsController(
		CreateHabitUseCase createHabitUseCase)
		: ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> Create([FromBody] CreateHabitCommand command)
		{
			var result = await createHabitUseCase.HandleAsync(command);

			if (result.IsSuccess())
				return Ok(result);

			if (result.IsRejected())
				return BadRequest(result);

			return StatusCode(StatusCodes.Status500InternalServerError, result);
		}
	}
}
