using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users
{
	public class UserRepository : IUserRepository
	{
		private readonly IMongoCollection<User> _users;

		public UserRepository(IDatabaseConfig databaseConfig)
		{
			var client = new MongoClient(databaseConfig.ConnectionString);
			var database = client.GetDatabase(databaseConfig.DatabaseName);
			_users = database.GetCollection<User>("users");
		}

		public async Task AddAsync(User user)
		{
			await _users.InsertOneAsync(user);
		}

		public async Task DeleteAsync(User user)
		{
			await _users.DeleteOneAsync(u => u.Id == user.Id);
		}

		public async Task<List<User>> GetAllAsync()
		{
			return (await _users.FindAsync(_ => true)).ToList();
		}

		public async Task<User> GetByIdAsync(string id)
		{
			return (await _users.FindAsync(user => user.Id == id)).FirstOrDefault();
		}

		public async Task UpdateAsync(User user)
		{
			await _users.ReplaceOneAsync(u => u.Id == user.Id, user);
		}
	}
}
