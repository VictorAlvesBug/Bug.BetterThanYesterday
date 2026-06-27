namespace Bug.BetterThanYesterday.Domain.Uploads;

public interface IPresignedUploadUrlGenerator
{
	(string UploadUrl, string FileUrl) Generate(string fileName, string contentType);
}
