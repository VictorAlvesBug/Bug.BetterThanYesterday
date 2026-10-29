namespace Bug.BetterThanYesterday.Application.Challenges.Models;

public class ChallengeRankingItemModel
{
	public int Position { get; set; }
	public Guid UserId { get; set; }
	public string UserName { get; set; } = string.Empty;
	public int CheckinCount { get; set; }
	public int PendingCheckinCount { get; set; }
	public decimal Penalty { get; set; }
	public int Streak { get; set; }
	public string? PhotoUrl { get; set; }
	public List<RankingDayModel> Days { get; set; } = [];
}
