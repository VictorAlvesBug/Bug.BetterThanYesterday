using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserUseCase(IUserRepository userRepository)
	: IUseCase<RegisterUserCommand, IResult>
{
	public async Task<IResult> HandleAsync(RegisterUserCommand input)
	{
		try
		{
			var alreadyExists = (await userRepository.GetByEmailAsync(input.Email)) is not null;

			if (alreadyExists)
			{
				return Result.Rejected("E-mail já cadastrado");
			}

			var user = User.CreateNew(input.Name, input.Email);
			await userRepository.AddAsync(user);
			return Result.Success(user.ToModel(), "Usuário cadastrado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
