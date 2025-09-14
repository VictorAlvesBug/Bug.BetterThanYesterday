namespace Bug.BetterThanYesterday.Application.Users.RegisterUser
{
	public class RegisterUserCommand
	{
		public RegisterUserCommand(
			string name,
			string email)
		{
			Name = name;
			Email = email;
		}

		public string Name { get; }
		public string Email { get; }
	}
}
