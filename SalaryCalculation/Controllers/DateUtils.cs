using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Controllers
{
    public class DateUtils
    {
        public static int GetFullYearsBetweenDates(DateTime startDate, DateTime endDate)
        {
            int result = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-result))
            {
                result--;
            }
            return result;
        }
    }
}
