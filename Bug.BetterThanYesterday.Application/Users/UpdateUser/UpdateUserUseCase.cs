using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserUseCase(IUserRepository userRepository)
	: IUseCase<UpdateUserCommand, IResult>
{
	public async Task<IResult> HandleAsync(UpdateUserCommand input)
	{
		try
		{
			var user = await userRepository.GetByIdAsync(input.Id);

			if (user is null)
				return Result.Rejected("Usuário não encontrado");

			user.UpdateName(input.Name);
			user.UpdateEmail(input.Email);
			
			await userRepository.UpdateAsync(user);
			return Result.Success(user.ToModel(), "Usuário atualizado com sucesso");
		}
		catch (Exception ex)
		{
			return Result.Failure(ex.Message);
		}
	}
}
