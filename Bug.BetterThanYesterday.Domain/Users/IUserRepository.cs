using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Domain.Users;

public interface IUserRepository : IRepository<User>
{
	Task<User?> GetByEmailAsync(Email email);
}
