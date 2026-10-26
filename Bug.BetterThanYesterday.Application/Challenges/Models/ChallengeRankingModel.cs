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

public class RankingDayModel
{
	public DateTime Date { get; set; }
	public string Position { get; set; } = string.Empty;
	public string Tone { get; set; } = "gray-e";
}

public class ChallengeRankingModel
{
	public int TotalCheckinCount { get; set; }
	public int DaysOffAvailable { get; set; }
	public List<ChallengeRankingItemModel> Items { get; set; } = [];
	public ChallengeRankingItemModel? CurrentUser { get; set; }
}
