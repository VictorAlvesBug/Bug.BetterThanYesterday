using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Plans;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public sealed class GetCheckInDetailsUseCase(
    ICheckInRepository checkInRepository,
    IPlanRepository planRepository,
    IUserRepository userRepository)
    : IUseCase<GetCheckInDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInDetailsCommand command)
    {
        try
        {
            command.Validate();

            var plan = await planRepository.GetByIdAsync(command.PlanId);
            if (plan is null)
                return Result.Rejected(Messages.PlanNotFound);

            var user = await userRepository.GetByIdAsync(command.UserId);
            if (user is null)
                return Result.Rejected(Messages.UserNotFound);

            var checkIn = await checkInRepository.GetDetailsAsync(
                command.PlanId,
                command.UserId,
                DateOnly.FromDateTime(command.Date),
                command.Index
            );

            if (checkIn is null)
                return Result.Rejected(Messages.CheckInNotFound);

            return Result.Success(checkIn.ToCheckInModel(), Messages.CheckInSuccessfullyFound);
        }
        catch (Exception ex)
        {
            return Result.Rejected(ex.Message);
        }
    }
}