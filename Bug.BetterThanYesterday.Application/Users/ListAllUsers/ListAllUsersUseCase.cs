using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users.ListAllUsers;

public class ListAllUsersUseCase(
	IUserRepository userRepository,
	IModelMapper<User, UserModel> mapper)
: IUseCase<ListAllUsersCommand, Result<List<UserModel>>>
{
	public async Task<Result<List<UserModel>>> HandleAsync(ListAllUsersCommand input)
	{
		try
		{
			var users = (await userRepository.ListAllAsync()).ConvertAll(mapper.ToModel);
			return Result<List<UserModel>>.Success(users);
		}
		catch (Exception ex)
		{
			return Result<List<UserModel>>.Failure(ex.Message);
		}
	}
}
