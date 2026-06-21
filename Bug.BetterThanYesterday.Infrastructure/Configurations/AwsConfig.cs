using Bug.BetterThanYesterday.Domain.Configurations;

namespace Bug.BetterThanYesterday.Infrastructure.Configurations;

public class AwsConfig : IAwsConfig
{
	public string AccessKey { get; set; } = string.Empty;
	public string SecretKey { get; set; } = string.Empty;
	public string Region { get; set; } = "sa-east-1";
	public string BucketName { get; set; } = string.Empty;
}
