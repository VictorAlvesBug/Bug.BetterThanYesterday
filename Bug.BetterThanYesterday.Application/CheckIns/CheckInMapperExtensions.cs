using System;
using System.Collections.Generic;
using System.Linq;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Habits;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
using Bug.BetterThanYesterday.Domain.Habits.Entities;
using Bug.BetterThanYesterday.Domain.Plans.Entities;
using Bug.BetterThanYesterday.Domain.Users.Entities;

namespace Bug.BetterThanYesterday.Application.CheckIns;

public static class CheckInMapperExtensions
{
    public static CheckInModel ToCheckInModel(this CheckIn checkIn) => new()
    {
        CheckInId = checkIn.Id,
        PlanId = checkIn.PlanId,
        UserId = checkIn.UserId,
        Date = checkIn.Date.ToDateTime(TimeOnly.MinValue),
        Index = checkIn.Index,
        Title = checkIn.Title,
        Description = checkIn.Description,
        CreatedAt = checkIn.CreatedAt
    };

    public static PlanWithCheckInsModel ToPlanWithCheckInsModel(
        this Plan plan,
        Habit habit,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(habit),
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };

    public static PlanUserWithCheckInsModel ToPlanUserWithCheckInsModel(
        this Plan plan,
        Habit habit,
        User user,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(habit),
        User = user.ToModel(),
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };

    public static PlanUserDateWithCheckInsModel ToPlanUserDateWithCheckInsModel(
        this Plan plan,
        Habit habit,
        User user,
        DateTime date,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(habit),
        User = user.ToModel(),
        Date = date,
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };
}