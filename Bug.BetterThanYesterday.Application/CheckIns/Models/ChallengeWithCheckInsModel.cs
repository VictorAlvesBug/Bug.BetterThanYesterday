using System.Collections.Generic;
using Bug.BetterThanYesterday.Application.Challenges;

namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class ChallengeWithCheckInsModel
{
    public required ChallengeModel Challenge { get; set; }
    public required List<CheckInModel> CheckIns { get; set; }
}