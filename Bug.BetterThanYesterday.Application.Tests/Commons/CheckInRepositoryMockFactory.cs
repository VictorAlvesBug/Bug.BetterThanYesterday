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
            .ReturnsAsync((Guid challengeId, Guid userId, DateOnly date, int index) =>
                checkIns.FirstOrDefault(x =>
                    x.ChallengeId == challengeId &&
                    x.UserId == userId &&
                    DateOnly.FromDateTime(x.Date) == date &&
                    x.Index == index
                )!
            );

        checkInRepository
            .Setup(repo => repo.ListByChallengeIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid challengeId) =>
                checkIns.Where(x => x.ChallengeId == challengeId).ToList()
            );

        checkInRepository
            .Setup(repo => repo.ListByChallengeIdAndUserIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>()
            ))
            .ReturnsAsync((Guid challengeId, Guid userId) =>
                checkIns.Where(x => x.ChallengeId == challengeId && x.UserId == userId).ToList()
            );

        checkInRepository
            .Setup(repo => repo.ListByChallengeIdAndUserIdAndDateAsync(
                It.IsAny<Guid>(),
                It.IsAny<Guid>(),
                It.IsAny<DateOnly>()
            ))
            .ReturnsAsync((Guid challengeId, Guid userId, DateOnly date) =>
                checkIns.Where(x =>
                    x.ChallengeId == challengeId &&
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
