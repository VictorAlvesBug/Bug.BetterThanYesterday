using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bug.BetterThanYesterday.Application.CheckIns.Models
{
    public class ReviewModel
    {
        public Guid ReviewerId { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
    }
}