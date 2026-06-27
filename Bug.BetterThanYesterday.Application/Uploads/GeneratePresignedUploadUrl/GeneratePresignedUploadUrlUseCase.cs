using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Configurations;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Uploads;

namespace Bug.BetterThanYesterday.Application.Uploads.GeneratePresignedUploadUrl;

public sealed class GeneratePresignedUploadUrlUseCase(
	IPresignedUploadUrlGenerator presignedUploadUrlGenerator,
	IAwsConfig awsConfig)
	: IUseCase<GeneratePresignedUploadUrlCommand>
{
	public Task<IResult> HandleAsync(GeneratePresignedUploadUrlCommand command)
	{
		try
		{
			command.Validate();

			if (string.IsNullOrWhiteSpace(awsConfig.AccessKey)
				|| string.IsNullOrWhiteSpace(awsConfig.SecretKey)
				|| string.IsNullOrWhiteSpace(awsConfig.BucketName))
				return Task.FromResult<IResult>(Result.Rejected(Messages.AwsConfigNotConfigured));

			var (uploadUrl, fileUrl) = presignedUploadUrlGenerator.Generate(
				command.FileName.Trim(),
				command.ContentType.Trim());

			return Task.FromResult<IResult>(Result.Success(
				new PresignedUploadUrlModel
				{
					UploadUrl = uploadUrl,
					FileUrl = fileUrl
				},
				Messages.PresignedUploadUrlSuccessfullyGenerated));
		}
		catch (Exception ex)
		{
			return Task.FromResult<IResult>(Result.Rejected(ex.Message));
		}
	}
}
