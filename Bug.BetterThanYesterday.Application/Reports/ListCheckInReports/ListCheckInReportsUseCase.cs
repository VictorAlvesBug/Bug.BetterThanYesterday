using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.Reports.ListCheckInReports;

public sealed class ListCheckInReportsUseCase(
	IChallengeRepository challengeRepository,
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

			var challenge = await challengeRepository.GetByIdAsync(command.ChallengeId);

			if (challenge is null)
				return Result.Rejected(Messages.ChallengeNotFound, RejectionType.NotFound);

			var isOwner = challenge.OwnerId == command.RequesterId;

			if (!command.IsPlatformAdmin && !isOwner)
				return Result.Rejected(Messages.OnlyOwnerOrAdminCanSeeReports);

			var reports = await checkInReportRepository.ListByChallengeIdAsync(command.ChallengeId);
			var checkIns = await checkInRepository.ListByChallengeIdAsync(command.ChallengeId);
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
						report.ChallengeId,
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
