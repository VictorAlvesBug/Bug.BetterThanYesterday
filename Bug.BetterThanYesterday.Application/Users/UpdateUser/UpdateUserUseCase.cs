using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserUseCase(IUserRepository userRepository)
	: IUseCase<UpdateUserCommand>
{
	public async Task<IResult> HandleAsync(UpdateUserCommand command)
	{
		command.Validate();

		var user = await userRepository.GetByIdAsync(command.UserId);

		if (user is null)
			return Result.Rejected(Messages.UserNotFound);

		var existingEmailUser = await userRepository.GetByEmailAsync(Email.Create(command.Email));

		if (existingEmailUser is not null
			&& existingEmailUser.Id != user.Id)
		{
			return Result.Rejected(Messages.EmailAlreadyRegistered);
		}

		user.UpdateName(command.Name);
		user.UpdateEmail(command.Email);

		await userRepository.UpdateAsync(user);
		return Result.Success(user.ToModel(), Messages.UserSuccessfullyUpdated);
	}
}
