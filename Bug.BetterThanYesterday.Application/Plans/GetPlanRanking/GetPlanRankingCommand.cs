using Bug.BetterThanYesterday.Application.SeedWork;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Plans.GetPlanRanking;

public class GetPlanRankingCommand : ICommand
{
	public GetPlanRankingCommand(Guid planId, Guid? userId = null)
	{
		PlanId = planId;
		UserId = userId;
	}

	public Guid PlanId { get; }
	public Guid? UserId { get; }

	public void Validate()
	{
		if (PlanId == Guid.Empty)
			throw new ArgumentNullException(nameof(PlanId), Messages.EnterPlanId);
	}
}
