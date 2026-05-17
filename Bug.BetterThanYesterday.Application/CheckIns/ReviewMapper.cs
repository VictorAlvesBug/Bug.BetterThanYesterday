using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bug.BetterThanYesterday.Application.CheckIns.Models;
using Bug.BetterThanYesterday.Domain.CheckIns.ValueObjects;

namespace Bug.BetterThanYesterday.Application.CheckIns;

public static class ReviewMapper
{
    public static ReviewModel ToModel(this Review review) => new()
    {
        ReviewerId = review.ReviewerId,
        Status = review.Status.Name,
        Date = review.Date
    };
}