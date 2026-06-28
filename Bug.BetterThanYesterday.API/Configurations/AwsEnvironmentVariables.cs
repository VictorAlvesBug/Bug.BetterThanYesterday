namespace Bug.BetterThanYesterday.API.Configurations;

internal static class AwsEnvironmentVariables
{
	private static readonly string[] AccessKeyNames =
	[
		"AWS_ACCESS_KEY",
		"AWS_ACCESS_KEY_ID",
	];

	private static readonly string[] SecretKeyNames =
	[
		"AWS_SECRET_KEY",
		"AWS_SECRET_ACCESS_KEY",
	];

	internal static string? ResolveAccessKey() => ResolveFirst(AccessKeyNames);

	internal static string? ResolveSecretKey() => ResolveFirst(SecretKeyNames);

	private static string? ResolveFirst(IEnumerable<string> names)
	{
		foreach (var name in names)
		{
            var value = Resolve(name);
            
            if (!string.IsNullOrWhiteSpace(value))
				return value;
		}

		return null;
	}

	private static string? Resolve(string name) =>
		Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process)
		?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User)
		?? Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Machine);
}
