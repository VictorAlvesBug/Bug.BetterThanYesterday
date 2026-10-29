namespace Bug.BetterThanYesterday.Application.Challenges.Models;

public class ChallengeRankingModel
{
	public int TotalCheckinCount { get; set; }
	public int DaysOffAvailable { get; set; }
	public List<ChallengeRankingItemModel> Items { get; set; } = [];
	public ChallengeRankingItemModel? CurrentUser { get; set; }
}
