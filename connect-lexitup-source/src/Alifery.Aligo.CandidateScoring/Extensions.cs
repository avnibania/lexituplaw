using System;

namespace Alifery.Aligo.CandidateScoring
{
    public static class Extensions
    {
        public static int GetMonthDifference(this DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}
