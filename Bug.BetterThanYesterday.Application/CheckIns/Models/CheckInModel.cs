namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class CheckInModel
{
    public required Guid Id { get; set; }
    public required Guid ChallengeId { get; set; }
    public required string ChallengeName { get; set; }
    public required Guid UserId { get; set; }
    public required string UserName { get; set; }
    public string? UserPhotoUrl { get; set; }
    public required DateTime Date { get; set; }
    public required int Index { get; set; }
    public required string Title { get; set; }
    public required string EvidencePhotoUrl { get; set; }
    public required string Status { get; set; }
    public required ReviewModel[] Reviews { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ReviewClosesAt { get; set; }
    public required bool IsReplacement { get; set; }
}
