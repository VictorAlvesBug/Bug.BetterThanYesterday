namespace Bug.BetterThanYesterday.Application.CheckIns;

internal class CheckInModel
{
	public Guid CheckInId { get; set; }
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateTime Date { get; set; }
	public int Index { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }
}
