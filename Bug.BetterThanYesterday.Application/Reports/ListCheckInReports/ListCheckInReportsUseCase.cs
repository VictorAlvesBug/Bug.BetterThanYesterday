using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;

public sealed class ListCheckInReportsUseCase(
	IPlanRepository planRepository,
	ICheckInReportRepository checkInReportRepository)
	: IUseCase<ListCheckInReportsCommand>
{
	public async Task<IResult> HandleAsync(ListCheckInReportsCommand command)
	{
		try
		{
			command.Validate();

			var plan = await planRepository.GetByIdAsync(command.PlanId);

			if (plan is null)
				return Result.Rejected(Messages.PlanNotFound, RejectionType.NotFound);

			var isOwner = plan.OwnerId == command.RequesterId;

			if (!command.IsPlatformAdmin && !isOwner)
				return Result.Rejected(Messages.OnlyOwnerOrAdminCanSeeReports);

			var reports = await checkInReportRepository.ListByPlanIdAsync(command.PlanId);

			return Result.Success(
				reports.Select(report => new
				{
					report.Id,
					report.CheckInId,
					report.PlanId,
					report.ReporterId,
					report.TargetUserId,
					report.Reason,
					report.CreatedAt
				}),
				Messages.ReportsSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
