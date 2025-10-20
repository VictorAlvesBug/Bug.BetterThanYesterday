using Bug.BetterThanYesterday.Application.SeedWork.UseCaseStructure;
using Bug.BetterThanYesterday.Domain.CheckIns;

namespace Bug.BetterThanYesterday.Application.CheckIns.AddCheckIn;

public sealed class AddCheckInUseCase(
    ICheckInRepository checkInRepository)
    : IUseCase<AddCheckInCommand>
{
    public Task<IResult> HandleAsync(AddCheckInCommand command)
    {
        throw new NotImplementedException();
    }
}