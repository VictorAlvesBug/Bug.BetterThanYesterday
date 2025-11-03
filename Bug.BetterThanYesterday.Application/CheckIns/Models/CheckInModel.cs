namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class CheckInModel
{
    public required Guid CheckInId { get; set; }
    public required Guid PlanId { get; set; }
    public required Guid UserId { get; set; }
    public required DateTime Date { get; set; }
    public required int Index { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}