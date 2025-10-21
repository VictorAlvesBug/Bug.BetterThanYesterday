using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.GetUserById;

public class GetUserByIdUseCase(IUserRepository userRepository)
	: IUseCase<GetUserByIdCommand>
{
	public async Task<IResult> HandleAsync(GetUserByIdCommand command)
	{
		command.Validate();
		var user = await userRepository.GetByIdAsync(command.UserId);

		if (user is null)
			return Result.Rejected(Messages.UserNotFound);

		return Result.Success(
			user.ToModel(),
			Messages.UserSuccessfullyFound
		);
	}
}
