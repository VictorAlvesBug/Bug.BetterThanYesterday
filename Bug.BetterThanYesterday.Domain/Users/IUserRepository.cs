using Bug.BetterThanYesterday.Domain.Commons;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Domain.Users;

public interface IUserRepository : IRepository<User>
{
	Task<User?> GetByEmailAsync(string email);
}
