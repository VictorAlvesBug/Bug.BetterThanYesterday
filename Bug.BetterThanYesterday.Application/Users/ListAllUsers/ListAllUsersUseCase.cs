using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.ListAllUsers;

public class ListAllUsersUseCase(IUserRepository userRepository)
	: IUseCase<ListAllUsersCommand>
{
	public async Task<IResult> HandleAsync(ListAllUsersCommand command)
	{
		command.Validate();
		var users = (await userRepository.ListAllAsync()).Select(user => user.ToModel());
		return Result.Success(
			users,
			Messages.UsersSuccessfullyFound
		);
	}
}
