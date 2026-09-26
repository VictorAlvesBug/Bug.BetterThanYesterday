using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Bug.BetterThanYesterday.API.Configurations;

internal sealed class AwsConfigEnvironmentConfigurer : IConfigureOptions<AwsConfig>
{
	public void Configure(AwsConfig options)
	{
		var accessKey = Environment.GetEnvironmentVariable("AWS_BTY_ACCESS_KEY");
		var secretKey = Environment.GetEnvironmentVariable("AWS_BTY_SECRET_KEY");
		var bucketName = Environment.GetEnvironmentVariable("AWS_BTY_S3_BUCKET");

		if (string.IsNullOrWhiteSpace(accessKey))
		{
			throw new InvalidOperationException(
				"Variável de ambiente AWS_BTY_ACCESS_KEY não configurada.");
		}
		
		if (string.IsNullOrWhiteSpace(secretKey))
		{
			throw new InvalidOperationException(
				"Variável de ambiente AWS_BTY_SECRET_KEY não configurada.");
		}
		
		if (string.IsNullOrWhiteSpace(bucketName))
		{
			throw new InvalidOperationException(
				"Variável de ambiente AWS_BTY_S3_BUCKET não configurada.");
		}

			options.AccessKey = accessKey;
			options.SecretKey = secretKey;
			options.BucketName = bucketName;
	}
}
