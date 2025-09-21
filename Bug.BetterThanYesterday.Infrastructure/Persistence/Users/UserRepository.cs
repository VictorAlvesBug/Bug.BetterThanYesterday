using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;
using Bug.BetterThanYesterday.Infrastructure.Persistence.Commons;
using MongoDB.Driver;

namespace Bug.BetterThanYesterday.Infrastructure.Persistence.Users;

public class UserRepository(
	IMongoCollection<UserDocument> collection,
	IDocumentMapper<User, UserDocument> mapper)
	: Repository<User, UserDocument>(
		collection,
		mapper), IUserRepository
{
	public async Task<User?> GetByEmailAsync(Email email)
	{
		var document = (await _collection.FindAsync(user => user.Email == email.Value)).FirstOrDefault();
		return document is null ? null : _mapper.ToDomain(document);
	}
}
