using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;

public sealed class ListCheckInReportsUseCase(
	IPlanRepository planRepository,
	ICheckInReportRepository checkInReportRepository,
	ICheckInRepository checkInRepository,
	IUserRepository userRepository)
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
			var checkIns = await checkInRepository.ListByPlanIdAsync(command.PlanId);
			var reporterIds = reports.Select(report => report.ReporterId).Distinct().ToList();
			var reporters = reporterIds.Count == 0
				? []
				: await userRepository.BatchGetByIdAsync(reporterIds);

			return Result.Success(
				reports.Select(report =>
				{
					var checkIn = checkIns.FirstOrDefault(item => item.Id == report.CheckInId);
					var reporter = reporters.FirstOrDefault(user => user.Id == report.ReporterId);
					return new
					{
						report.Id,
						report.CheckInId,
						report.PlanId,
						report.ReporterId,
						ReporterName = reporter?.Nickname ?? "Alguém",
						report.TargetUserId,
						report.Reason,
						report.CreatedAt,
						CheckInTitle = checkIn?.Title ?? "Check-in",
						EvidencePhotoUrl = checkIn?.PhotoUrl ?? string.Empty
					};
				}),
				Messages.ReportsSuccessfullyFound);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
