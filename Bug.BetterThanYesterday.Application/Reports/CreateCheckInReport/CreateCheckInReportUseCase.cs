using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.ChallengeMembers;
using Bug.BetterThanYesterday.Domain.ChallengeMembers.Entities;
using Bug.BetterThanYesterday.Domain.Challenges;
using Bug.BetterThanYesterday.Domain.Reports;
using Bug.BetterThanYesterday.Domain.Reports.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;
using Microsoft.Extensions.Options;

namespace Bug.BetterThanYesterday.Application.Reports.CreateCheckInReport;

public sealed class PlatformAdminOptions
{
	public const string SectionName = "Admin";
	public string UserId { get; set; } = string.Empty;
}

public sealed class CreateCheckInReportUseCase(
	ICheckInRepository checkInRepository,
	IChallengeMemberRepository challengeMemberRepository,
	ICheckInReportRepository checkInReportRepository,
	IChallengeRepository challengeRepository,
	IUserRepository userRepository,
	IChallengeCycleNotificationProcessor notificationProcessor,
	IOptions<PlatformAdminOptions> adminOptions)
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

			var reporterMemberId = ChallengeMember.BuildId(checkIn.ChallengeId, command.ReporterId);
			var reporter = await challengeMemberRepository.GetByIdAsync(reporterMemberId);

			if (reporter is null)
				return Result.Rejected(Messages.UserIsNotInTheChallenge, RejectionType.NotFound);

			var report = CheckInReport.CreateNew(
				checkIn.Id,
				checkIn.ChallengeId,
				command.ReporterId,
				checkIn.UserId,
				command.Reason);

			await checkInReportRepository.AddAsync(report);

			try
			{
				var challenge = await challengeRepository.GetByIdAsync(checkIn.ChallengeId);
				var reporterUser = await userRepository.GetByIdAsync(command.ReporterId);
				var targetUser = await userRepository.GetByIdAsync(checkIn.UserId);
				var reporterName = reporterUser?.Nickname ?? "Alguém";
				var targetName = targetUser?.Nickname ?? "um membro";
				var message = $"{reporterName} denunciou {targetName} na arena. Motivo: {command.Reason.Trim()}";
				var recipients = new List<Guid>();

				if (challenge is not null)
					recipients.Add(challenge.OwnerId);

				if (Guid.TryParse(adminOptions.Value?.UserId, out var adminId))
					recipients.Add(adminId);

				await notificationProcessor.NotifyCheckInReportedAsync(
					checkIn.ChallengeId,
					checkIn.Id,
					command.ReporterId,
					checkIn.UserId,
					message,
					recipients);
			}
			catch
			{
				// O aviso não impede a denúncia.
			}

			return Result.Success(new
			{
				report.Id,
				report.CheckInId,
				report.ChallengeId,
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
