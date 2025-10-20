using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.Strings;

namespace Bug.BetterThanYesterday.Application.CheckIns.GetCheckInDetails;

public sealed class GetCheckInDetailsUseCase(
    ICheckInRepository checkInRepository)
    : IUseCase<GetCheckInDetailsCommand>
{
    public async Task<IResult> HandleAsync(GetCheckInDetailsCommand command)
    {
        command.Validate();

        var checkIn = await checkInRepository.GetDetailsAsync(
            command.PlanId,
            command.UserId,
            DateOnly.FromDateTime(command.Date),
            command.Index
        );

        if (checkIn is null)
            return Result.Rejected(Messages.CheckInNotFound);

        return Result.Success(checkIn);
    }
}