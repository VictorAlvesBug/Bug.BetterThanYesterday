using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.DeleteUser;

public class DeleteUserUseCase(IUserRepository userRepository)
	: IUseCase<DeleteUserCommand>
{
	public async Task<IResult> HandleAsync(DeleteUserCommand command)
	{
		command.Validate();
		var user = await userRepository.GetByIdAsync(command.UserId);

		if (user is null)
			return Result.Rejected(Messages.UserNotFound);

		await userRepository.DeleteAsync(user);
		return Result.Success(Messages.UserSuccessfullyDeleted);
	}
}
