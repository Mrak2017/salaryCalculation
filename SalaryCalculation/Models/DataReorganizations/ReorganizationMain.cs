using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models.DataReorganizations
{
    public abstract class ReorganizationMain
    {
        protected readonly SalaryCalculationDBContext context;

        public ReorganizationMain(SalaryCalculationDBContext context)
        {
            this.context = context;
        }

        public abstract void Run();
    }
}
