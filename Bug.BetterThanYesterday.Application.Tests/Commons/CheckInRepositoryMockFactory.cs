using Bug.BetterThanYesterday.Domain.CheckIns;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Moq;

namespace Bug.BetterThanYesterday.Application.Tests.Commons;

public static class CheckInRepositoryMockFactory
{
    public static readonly Guid CheckInId1 = Guid.Parse("d7c9f9d3-2b77-4c2c-a8d1-9b6f2b3d1a11");
    public static readonly Guid CheckInId2 = Guid.Parse("a13b9c7f-5f9a-4a2e-8b2c-3d1f4e5a2b22");
    public static readonly Guid CheckInId3 = Guid.Parse("c2f3b7e8-6d8f-4b1a-9c3d-7f2a1b4c3d33");

    public static (Mock<ICheckInRepository> repo, List<CheckIn> data) Create()
    {
        var checkIns = new List<CheckIn>
        {
                CheckIn.Restore(
                    CheckInId1,
                    PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active,
                    UserRepositoryMockFactory.UserId1,
                    new DateTime(2025, 01, 05),
                    0,
                    "Morning workout",
                    "Did 30 minutes of cardio"
                ),
                CheckIn.Restore(
                    CheckInId2,
                    PlanRepositoryMockFactory.PublicRunningPlanId1_WithUserId1Active,
                    UserRepositoryMockFactory.UserId2,
                    new DateTime(2025, 01, 05),
                    0,
                    "Reading",
                    "Read 15 pages"
                ),
                CheckIn.Restore(
                    CheckInId3,
                    PlanRepositoryMockFactory.PublicRunningPlanId7_WithUserId3BlockedAndUserId4LeftAndUserId5Active,
                    UserRepositoryMockFactory.UserId3,
                    new DateTime(2025, 10, 10),
                    1,
                    "Evening review",
                    "Reviewed notes"
                )
        };

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
                    x.Date == date &&
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
                checkIns.Where(x => x.PlanId == planId && x.UserId == userId && x.Date == date).ToList()
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
