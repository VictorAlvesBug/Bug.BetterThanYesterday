using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Users.UpdateUser;

public class UpdateUserUseCase(IUserRepository userRepository)
	: IUseCase<UpdateUserCommand>
{
	public async Task<IResult> HandleAsync(UpdateUserCommand command)
	{
		command.Validate();

		var user = await userRepository.GetByIdAsync(command.Id);

		if (user is null)
			return Result.Rejected("Usuário não encontrado");

		var existingEmailUser = await userRepository.GetByEmailAsync(Email.Create(command.Email));

		if (existingEmailUser is not null
			&& existingEmailUser.Id != user.Id)
		{
			return Result.Rejected("E-mail já cadastrado");
		}

		user.UpdateName(command.Name);
		user.UpdateEmail(command.Email);

		await userRepository.UpdateAsync(user);
		return Result.Success(user.ToModel(), "Usuário atualizado com sucesso");
	}
}
