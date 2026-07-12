using Bug.BetterThanYesterday.Application.Mocks;
using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class CheckInRepositoryMockFactory
{
    public static (Mock<ICheckInRepository> repo, List<CheckIn> data) Create()
    {
        var checkIns = MockData.MockCheckIns;

        var checkInRepository = new Mock<ICheckInRepository>();

        checkInRepository.Setup(repo => repo.AddAsync(It.IsAny<CheckIn>()));

        checkInRepository
            .Setup(repo => repo.ListAllAsync())
            .ReturnsAsync(checkIns);

        checkInRepository
            .Setup(repo => repo.UpdateAsync(It.IsAny<CheckIn>()));

        checkInRepository
            .Setup(repo => repo.DeleteAsync(It.IsAny<CheckIn>()));

        checkInRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()));

        checkInRepository
            .Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()));

        checkInRepository
            .Setup(repo => repo.GetDetailsAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<DateOnly>(),
                It.IsAny<int>()
            ))
            .ReturnsAsync((Guid planId, Guid userId, DateOnly date, int index) =>
                checkIns.FirstOrDefault(x =>
                    x.PlanId == planId &&
                    x.UserId == userId &&
                    DateOnly.FromDateTime(x.Date) == date &&
                    x.Index == index
                )!
            );

        checkInRepository
            .Setup(repo => repo.ListByPlanIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid planId) =>
                checkIns.Where(x => x.PlanId == planId).ToList()
            );

        checkInRepository
            .Setup(repo => repo.ListByPlanIdAndUserIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()
            ))
            .ReturnsAsync((Guid planId, Guid userId) =>
                checkIns.Where(x => x.PlanId == planId && x.UserId == userId).ToList()
            );

        checkInRepository
            .Setup(repo => repo.ListByPlanIdAndUserIdAndDateAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<DateOnly>()
            ))
            .ReturnsAsync((Guid planId, Guid userId, DateOnly date) =>
                checkIns.Where(x =>
                    x.PlanId == planId &&
                    x.UserId == userId &&
                    DateOnly.FromDateTime(x.Date) == date).ToList()
            );

        // GetByIdAsync should return matching checkin by id
        checkInRepository
            .Setup(repo => repo.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => checkIns.Find(c => c.Id == id));

        // BatchGetByIdAsync returns matching checkins for provided ids
        checkInRepository
            .Setup(repo => repo.BatchGetByIdAsync(It.IsAny<List<Guid>>()))
            .ReturnsAsync((List<Guid> ids) => checkIns.Where(c => ids.Contains(c.Id)).ToList());

        return (checkInRepository, checkIns);
    }
}
