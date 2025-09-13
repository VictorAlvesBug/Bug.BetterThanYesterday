using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

		// GET: api/<UsersController>
		[HttpGet]
		public async Task<IEnumerable<User>> Get()
		{
			return await _userRepository.GetAllAsync();
		}

		// GET api/<UsersController>/5
		[HttpGet("{id}")]
		public async Task<User> Get(string id)
		{
			return await _userRepository.GetByIdAsync(id);
		}

		// POST api/<UsersController>
		[HttpPost]
		public async Task Post([FromBody] User user)
		{
			await _userRepository.AddAsync(user);
		}

		// PUT api/<UsersController>/5
		[HttpPut("{id}")]
		public async Task Put(int id, [FromBody] User user)
		{
			await _userRepository.UpdateAsync(user);
		}

		// DELETE api/<UsersController>/5
		[HttpDelete("{id}")]
		public async Task Delete(string id)
		{
			var user = await _userRepository.GetByIdAsync(id);
			await _userRepository.DeleteAsync(user);
		}
	}
}
