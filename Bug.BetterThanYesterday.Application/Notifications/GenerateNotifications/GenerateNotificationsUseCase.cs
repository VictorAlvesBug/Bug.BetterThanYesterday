using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.Notifications;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.Notifications.GenerateNotifications;

public sealed class GenerateNotificationsUseCase(
	IPlanCycleNotificationProcessor notificationProcessor,
	IUserNotificationRepository notificationRepository)
	: IUseCase<GenerateNotificationsCommand>
{
	public async Task<IResult> HandleAsync(GenerateNotificationsCommand command)
	{
		command.Validate();
		await notificationProcessor.GenerateForUserAsync(command.UserId);
		var notifications = await notificationRepository.ListByUserIdAsync(command.UserId);

		return Result.Success(
			notifications
				.OrderByDescending(notification => notification.CreatedAt)
				.Select(notification => new
				{
					notification.Id,
					notification.PlanId,
					Kind = notification.Kind.Name,
					notification.Message,
					notification.Payload,
					notification.CreatedAt
				}),
			Messages.NotificationsSuccessfullyFound);
	}
}
