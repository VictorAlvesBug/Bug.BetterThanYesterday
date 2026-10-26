using System;
using System.Collections.Generic;
using System.Linq;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Challenges.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

public static class CheckInMapperExtensions
{
    public static ChallengeWithCheckInsModel ToChallengeWithCheckInsModel(
        this Challenge challenge,
        Habit habit,
        User owner,
        List<CheckIn> checkIns,
        List<User> users)
    {
        var usersById = users.ToDictionary(user => user.Id);

        return new()
        {
            Challenge = challenge.ToModel(habit, owner),
            CheckIns = checkIns
                .Select(checkIn => checkIn.ToModel(
                    challenge,
                    habit,
                    usersById.GetValueOrDefault(checkIn.UserId) ?? throw new Exception(Messages.UserNotFound)))
                .ToList()
        };
    }

    public static ChallengeUserWithCheckInsModel ToChallengeUserWithCheckInsModel(
        this Challenge challenge,
        Habit habit,
        User owner,
        User user,
        List<CheckIn> checkIns) => new()
        {
            Challenge = challenge.ToModel(habit, owner),
            User = user.ToModel(),
            CheckIns = checkIns.Select(checkIn => checkIn.ToModel(challenge, habit, user)).ToList()
        };

    public static ChallengeUserDateWithCheckInsModel ToChallengeUserDateWithCheckInsModel(
        this Challenge challenge,
        Habit habit,
        User owner,
        User user,
        DateTime date,
        List<CheckIn> checkIns) => new()
        {
            Challenge = challenge.ToModel(habit, owner),
            User = user.ToModel(),
            Date = date,
            CheckIns = checkIns.Select(checkIn => checkIn.ToModel(challenge, habit, user)).ToList()
        };
}