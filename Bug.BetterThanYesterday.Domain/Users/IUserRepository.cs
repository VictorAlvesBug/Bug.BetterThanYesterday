using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Domain.Users
{
	public interface IUserRepository
	{
		Task<List<User>> GetAllAsync();
		Task<User> GetByIdAsync(string id);
		Task AddAsync(User user);
		Task UpdateAsync(User user);
		Task DeleteAsync(User user);
	}
}
