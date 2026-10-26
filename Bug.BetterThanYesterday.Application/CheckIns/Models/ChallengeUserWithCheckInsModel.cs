using System.Collections.Generic;
using Bug.BetterThanYesterday.Application.Challenges;
using Bug.BetterThanYesterday.Application.Users;

namespace Bug.BetterThanYesterday.Application.CheckIns.Models;

public class ChallengeUserWithCheckInsModel
{
    public required ChallengeModel Challenge { get; set; }
    public required UserModel User { get; set; }
    public required List<CheckInModel> CheckIns { get; set; }
}