namespace Bug.BetterThanYesterday.Domain.Configurations;

public interface IAwsConfig
{
	string AccessKey { get; set; }
	string SecretKey { get; set; }
	string Region { get; set; }
	string BucketName { get; set; }
}
