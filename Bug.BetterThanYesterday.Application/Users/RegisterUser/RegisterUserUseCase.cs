using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserUseCase(IUserRepository userRepository)
	: IUseCase<RegisterUserCommand>
{
	public async Task<IResult> HandleAsync(RegisterUserCommand command)
	{
		try
		{
			command.Validate();
			
			var alreadyExists = (await userRepository.GetByEmailAsync(Email.Create(command.Email))) is not null;

			if (alreadyExists)
				return Result.Rejected(Messages.UserEmailAlreadyRegistered);

			var user = User.CreateNew(command.Name, command.Email);
			await userRepository.AddAsync(user);
			return Result.Success(user.ToModel(), Messages.UserSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
