using System;
using System.Collections.Generic;
using Bug.BetterThanYesterday.Application.Plans;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class PlanUserDateWithCheckInsModel
{
    public required PlanModel Plan { get; set; }
    public required UserModel User { get; set; }
    public required DateTime Date { get; set; }
    public required List<CheckInModel> CheckIns { get; set; }
}