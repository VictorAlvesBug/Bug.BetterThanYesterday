using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Bug.BetterThanYesterday.Domain.Users.Entities;
using Bug.BetterThanYesterday.Domain.Users.ValueObjects;

namespace Bug.BetterThanYesterday.Application.Users.ListUsersByFilter;

public class ListUsersByFilterUseCase(IUserRepository userRepository)
	: IUseCase<ListUsersByFilterCommand>
{
	public async Task<IResult> HandleAsync(ListUsersByFilterCommand command)
	{
		try
		{
			command.Validate();
			
			List<User> users = [];

			if (!string.IsNullOrEmpty(command.Email))
			{
				var email = Email.Create(command.Email);
				var user = await userRepository.GetByEmailAsync(email);
				if(user is not null)
					users.Add(user);
			}
			else
			{
				users = await userRepository.ListAllAsync();
			}

			return Result.Success(users.Select(user => user.ToModel()));
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
