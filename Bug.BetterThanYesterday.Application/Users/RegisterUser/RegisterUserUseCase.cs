using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;

namespace Bug.BetterThanYesterday.Application.Users.RegisterUser;

public class RegisterUserUseCase : IUseCase<RegisterUserCommand, Result<UserModel>>
{
	public Task<Result<UserModel>> HandleAsync(RegisterUserCommand input)
	{
		throw new NotImplementedException();
	}
}
