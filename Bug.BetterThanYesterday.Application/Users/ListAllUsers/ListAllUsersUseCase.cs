using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.ListAllUsers;

public class ListAllUsersUseCase(IUserRepository userRepository)
	: IUseCase<ListAllUsersCommand, IResult>
{
	public async Task<IResult> HandleAsync(ListAllUsersCommand command)
	{
		try
		{
			command.Validate();
			var users = (await userRepository.ListAllAsync()).Select(user => user.ToModel());
			return Result.Success(users);
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
