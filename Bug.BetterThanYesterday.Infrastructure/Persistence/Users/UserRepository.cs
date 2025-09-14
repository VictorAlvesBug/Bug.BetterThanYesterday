using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Plans;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users
{
	public class UserRepository(
		IDatabaseConfig databaseConfig,
		IDocumentMapper<User, UserDocument> mapper)
		: Repository<User, UserDocument>(
			databaseConfig,
			"users",
			mapper), IUserRepository
	{
		public async Task<User?> GetByEmailAsync(string email)
		{
			var document = (await _collection.FindAsync(user => user.Email == email)).FirstOrDefault();
			return document is null ? null : _mapper.ToDomain(document);
		}
	}
}
