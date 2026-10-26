using Bug.BetterThanYesterday.Application.Notifications;
using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.PlanMembers;
using Bug.BetterThanYesterday.Domain.PlanMembers.Entities;
using Bug.BetterThanYesterday.Domain.Plans;
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
	IPlanMemberRepository planMemberRepository,
	ICheckInReportRepository checkInReportRepository,
	IPlanRepository planRepository,
	IUserRepository userRepository,
	IPlanCycleNotificationProcessor notificationProcessor,
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

			try
			{
				var plan = await planRepository.GetByIdAsync(checkIn.PlanId);
				var reporterUser = await userRepository.GetByIdAsync(command.ReporterId);
				var targetUser = await userRepository.GetByIdAsync(checkIn.UserId);
				var reporterName = reporterUser?.Nickname ?? "Alguém";
				var targetName = targetUser?.Nickname ?? "um membro";
				var message = $"{reporterName} denunciou {targetName}. Motivo: {command.Reason.Trim()}";
				var recipients = new List<Guid>();

				if (plan is not null)
					recipients.Add(plan.OwnerId);

				if (Guid.TryParse(adminOptions.Value?.UserId, out var adminId))
					recipients.Add(adminId);

				await notificationProcessor.NotifyCheckInReportedAsync(
					checkIn.PlanId,
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
