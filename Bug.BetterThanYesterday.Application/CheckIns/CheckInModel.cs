namespace Bug.BetterThanYesterday.Application.CheckIns;

internal class CheckInModel
{
	public Guid Id { get; set; }
	public Guid PlanId { get; set; }
	public Guid UserId { get; set; }
	public DateTime Date { get; set; }
	public string Title { get; set; }
	public string? Description { get; set; }
}
