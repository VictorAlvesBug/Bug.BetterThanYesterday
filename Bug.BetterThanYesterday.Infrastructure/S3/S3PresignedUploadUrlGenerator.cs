using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Uploads;
using Bug.BetterThanYesterday.Infrastructure.Configurations;
using Microsoft.Extensions.Options;

namespace Bug.BetterThanYesterday.Infrastructure.S3;

public sealed class S3PresignedUploadUrlGenerator(IOptions<AwsConfig> awsConfigOptions)
	: IPresignedUploadUrlGenerator
{
	public (string UploadUrl, string FileUrl) Generate(string fileName, string contentType)
	{
		IAwsConfig config = awsConfigOptions.Value;
		var region = RegionEndpoint.GetBySystemName(config.Region);

		using var client = new AmazonS3Client(config.AccessKey, config.SecretKey, region);

		var request = new GetPreSignedUrlRequest
		{
			BucketName = config.BucketName,
			Key = fileName,
			Verb = HttpVerb.PUT,
			Expires = DateTime.UtcNow.AddMinutes(15),
			ContentType = contentType
		};

		var uploadUrl = client.GetPreSignedURL(request);
		var fileUrl = $"https://{config.BucketName}.s3.{config.Region}.amazonaws.com/{fileName}";

		return (uploadUrl, fileUrl);
	}
}
