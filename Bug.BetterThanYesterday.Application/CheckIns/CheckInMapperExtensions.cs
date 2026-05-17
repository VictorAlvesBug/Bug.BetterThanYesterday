using System;
using System.Collections.Generic;
using System.Linq;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Strings;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

public static class CheckInMapperExtensions
{
    public static PlanWithCheckInsModel ToPlanWithCheckInsModel(
        this Plan plan,
        Habit habit,
        User owner,
        List<CheckIn> checkIns,
        List<User> users)
    {
        var usersById = users.ToDictionary(user => user.Id);

        return new()
        {
            Plan = plan.ToModel(habit, owner),
            CheckIns = checkIns
                .Select(checkIn => checkIn.ToModel(
                    plan,
                    habit,
                    usersById.GetValueOrDefault(checkIn.UserId) ?? throw new Exception(Messages.UserNotFound)))
                .ToList()
        };
    }

    public static PlanUserWithCheckInsModel ToPlanUserWithCheckInsModel(
        this Plan plan,
        Habit habit,
        User owner,
        User user,
        List<CheckIn> checkIns) => new()
        {
            Plan = plan.ToModel(habit, owner),
            User = user.ToModel(),
            CheckIns = checkIns.Select(checkIn => checkIn.ToModel(plan, habit, user)).ToList()
        };

    public static PlanUserDateWithCheckInsModel ToPlanUserDateWithCheckInsModel(
        this Plan plan,
        Habit habit,
        User owner,
        User user,
        DateTime date,
        List<CheckIn> checkIns) => new()
        {
            Plan = plan.ToModel(habit, owner),
            User = user.ToModel(),
            Date = date,
            CheckIns = checkIns.Select(checkIn => checkIn.ToModel(plan, habit, user)).ToList()
        };
}