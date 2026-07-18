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
		try
		{
			command.Validate();

			var user = await userRepository.GetByIdAsync(command.UserId);

			if (user is null)
				return Result.Rejected(Messages.UserNotFound, RejectionType.NotFound);

			if (!string.IsNullOrWhiteSpace(command.Email))
			{
				var existingEmailUser = await userRepository.GetByEmailAsync(Email.Create(command.Email));

				if (existingEmailUser is not null
					&& existingEmailUser.Id != user.Id)
				{
					return Result.Rejected(Messages.UserEmailAlreadyRegistered);
				}

				user.UpdateEmail(command.Email);
			}

			if (!string.IsNullOrWhiteSpace(command.Name))
				user.UpdateName(command.Name);

			if (!string.IsNullOrWhiteSpace(command.Nickname))
				user.UpdateNickname(command.Nickname);

			if (!string.IsNullOrWhiteSpace(command.PhoneNumber))
				user.UpdatePhoneNumber(command.PhoneNumber);

			if (!string.IsNullOrWhiteSpace(command.PhotoUrl))
				user.UpdatePhoto(command.PhotoUrl);

			if (!string.IsNullOrWhiteSpace(command.PixKey) && !string.IsNullOrWhiteSpace(command.PixKeyType))
				user.UpdatePixKey(command.PixKey, command.PixKeyType);

			await userRepository.UpdateAsync(user);
			return Result.Success(user.ToModel(), Messages.UserSuccessfullyUpdated);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
