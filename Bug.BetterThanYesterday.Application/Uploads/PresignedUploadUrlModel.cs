namespace Bug.BetterThanYesterday.Application.Uploads;

public class PresignedUploadUrlModel
{
	public required string UploadUrl { get; set; }
	public required string FileUrl { get; set; }
}
