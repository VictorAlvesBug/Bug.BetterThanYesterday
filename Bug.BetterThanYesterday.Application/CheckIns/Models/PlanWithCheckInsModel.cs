using System.Collections.Generic;
using Bug.BetterThanYesterday.Application.Plans;

namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class PlanWithCheckInsModel
{
    public required PlanModel Plan { get; set; }
    public required List<CheckInModel> CheckIns { get; set; }
}