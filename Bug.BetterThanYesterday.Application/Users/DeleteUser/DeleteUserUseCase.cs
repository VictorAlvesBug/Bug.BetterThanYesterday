using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.DeleteUser;

public class DeleteUserUseCase(IUserRepository userRepository)
	: IUseCase<DeleteUserCommand, IResult>
{
	public async Task<IResult> HandleAsync(DeleteUserCommand input)
	{
		try
		{
			var user = await userRepository.GetByIdAsync(input.Id);

			if (user is null)
				return Result.Rejected("Usuário não encontrado");

			await userRepository.DeleteAsync(user);
			return Result.Success("Usuário deletado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
