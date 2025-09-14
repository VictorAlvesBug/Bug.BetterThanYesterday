using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users
{
	public class UserRepository(IDatabaseConfig databaseConfig)
		: Repository<User>(databaseConfig, "users"), IUserRepository
	{
		public async Task<User> GetByEmailAsync(string email)
		{
			return (await _entities.FindAsync(user => user.Email.Value == email)).FirstOrDefault();
		}
	}
}
