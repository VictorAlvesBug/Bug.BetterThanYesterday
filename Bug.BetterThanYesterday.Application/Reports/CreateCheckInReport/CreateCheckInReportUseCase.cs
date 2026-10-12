using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Reports.Entities;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport;

public sealed class CreateCheckInReportUseCase(
	ICheckInRepository checkInRepository,
	IPlanMemberRepository planMemberRepository,
	ICheckInReportRepository checkInReportRepository)
	: IUseCase<CreateCheckInReportCommand>
{
	public async Task<IResult> HandleAsync(CreateCheckInReportCommand command)
	{
		try
		{
			command.Validate();

			var checkIn = await checkInRepository.GetByIdAsync(command.CheckInId);

			if (checkIn is null)
				return Result.Rejected(Messages.CheckInNotFound, RejectionType.NotFound);

			var reporterMemberId = PlanMember.BuildId(checkIn.PlanId, command.ReporterId);
			var reporter = await planMemberRepository.GetByIdAsync(reporterMemberId);

			if (reporter is null)
				return Result.Rejected(Messages.UserIsNotInThePlan, RejectionType.NotFound);

			var report = CheckInReport.CreateNew(
				checkIn.Id,
				checkIn.PlanId,
				command.ReporterId,
				checkIn.UserId,
				command.Reason);

			await checkInReportRepository.AddAsync(report);

			return Result.Success(new
			{
				report.Id,
				report.CheckInId,
				report.PlanId,
				report.ReporterId,
				report.TargetUserId,
				report.Reason,
				report.CreatedAt
			}, Messages.ReportSuccessfullyRegistered);
		}
		catch (Exception ex)
		{
			return Result.Rejected(ex.Message);
		}
	}
}
