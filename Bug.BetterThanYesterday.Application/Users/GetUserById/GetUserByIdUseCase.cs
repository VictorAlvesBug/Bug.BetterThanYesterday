using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.GetUserById;

public class GetUserByIdUseCase(IUserRepository userRepository)
	: IUseCase<GetUserByIdCommand, IResult>
{
	public async Task<IResult> HandleAsync(GetUserByIdCommand command)
	{
		try
		{
			command.Validate();
			var user = await userRepository.GetByIdAsync(command.Id);

			if (user is null)
				return Result.Rejected("Usuário não encontrado");

			return Result.Success(user.ToModel());
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
