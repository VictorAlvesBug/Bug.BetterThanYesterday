namespace Bug.BetterThanYesterday.Application.CheckIns;

internal class CheckInModel
{
	public string Id { get; set; }
	public string PlanId { get; set; }
	public string UserId { get; set; }
	public DateOnly Date { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }
}
