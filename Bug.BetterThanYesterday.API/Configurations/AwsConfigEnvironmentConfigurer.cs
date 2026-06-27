using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Bug.BetterThanYesterday.API.Configurations;

internal sealed class AwsConfigEnvironmentConfigurer : IConfigureOptions<AwsConfig>
{
	public void Configure(AwsConfig options)
	{
		var accessKey = AwsEnvironmentVariables.ResolveAccessKey();
		var secretKey = AwsEnvironmentVariables.ResolveSecretKey();

		if (!string.IsNullOrWhiteSpace(accessKey))
			options.AccessKey = accessKey;

		if (!string.IsNullOrWhiteSpace(secretKey))
			options.SecretKey = secretKey;
	}
}
