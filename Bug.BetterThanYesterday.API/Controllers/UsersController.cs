using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Bug.BetterThanYesterday.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UsersController : ControllerBase
	{
		private IUserRepository _userRepository;

		public UsersController(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpGet]
		public async Task<IActionResult> Get()
		{
			return Ok(await _userRepository.ListAllAsync());
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> Get(string id)
		{
			return Ok(await _userRepository.GetByIdAsync(id));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] User user)
		{
			await _userRepository.AddAsync(user);
			return Created();
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] User user)
		{
			await _userRepository.UpdateAsync(user);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userRepository.GetByIdAsync(id);
			await _userRepository.DeleteAsync(user);
			return NoContent();
		}
	}
}
