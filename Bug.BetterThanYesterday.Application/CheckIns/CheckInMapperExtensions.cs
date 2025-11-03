using System;
using System.Collections.Generic;
using System.Linq;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;
using Bug.BetterThanYesterday.Domain.CheckIns.Entities;
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
        Description = checkIn.Description
    };

    public static PlanWithCheckInsModel ToPlanWithCheckInsModel(
        this Plan plan,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(),
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };

    public static PlanUserWithCheckInsModel ToPlanUserWithCheckInsModel(
        this Plan plan,
        User user,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(),
        User = user.ToModel(),
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };

    public static PlanUserDateWithCheckInsModel ToPlanUserDateWithCheckInsModel(
        this Plan plan,
        User user,
        DateTime date,
        List<CheckIn> checkIns) => new()
    {
        Plan = plan.ToModel(),
        User = user.ToModel(),
        Date = date,
        CheckIns = checkIns.Select(x => x.ToCheckInModel()).ToList()
    };
}