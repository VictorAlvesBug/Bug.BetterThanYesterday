using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Uploads.GeneratePresignedUploadUrl;

public class GeneratePresignedUploadUrlCommand : ICommand
{
	public required string FileName { get; init; }
	public required string ContentType { get; init; }

	public void Validate()
	{
		if (string.IsNullOrWhiteSpace(FileName))
			throw new ArgumentException(Messages.EnterUploadFileName, nameof(FileName));

		if (string.IsNullOrWhiteSpace(ContentType))
			throw new ArgumentException(Messages.EnterUploadContentType, nameof(ContentType));
	}
}
