using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserUseCase(IUserRepository userRepository)
	: IUseCase<RegisterUserCommand, Result<UserModel>>
{
	public async Task<Result<UserModel>> HandleAsync(RegisterUserCommand input)
	{
		try
		{
			var alreadyExists = (await userRepository.GetByEmailAsync(input.Email)) is not null;

			if (alreadyExists)
			{
				return Result<UserModel>.Rejected("E-mail já cadastrado");
			}

			await userRepository.AddAsync(User.CreateNew(input.Name, input.Email));
			return Result<UserModel>.Success("Usuário cadastrado com sucesso");
		}
		catch (Exception ex)
		{
			return Result<UserModel>.Failure(ex.Message);
		}
	}
}
