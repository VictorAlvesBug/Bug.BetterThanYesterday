namespace Bug.BetterThanYesterday.Application.Plans.Models;

public class PlanRankingItemModel
{
	public int Position { get; set; }
	public Guid UserId { get; set; }
	public string UserName { get; set; } = string.Empty;
	public int CheckinCount { get; set; }
	public decimal Penalty { get; set; }
	public int Streak { get; set; }
}

public class PlanRankingModel
{
	public int TotalCheckinCount { get; set; }
	public int DaysOffAvailable { get; set; }
	public List<PlanRankingItemModel> Items { get; set; } = [];
	public PlanRankingItemModel? CurrentUser { get; set; }
}
