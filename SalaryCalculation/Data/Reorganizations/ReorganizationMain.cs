using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models.DataReorganizations
{
    /** Базовый класс для реорганизации данных. 
     * Каждая реорганизация = отдельный наследник*/
    public abstract class ReorganizationMain
    {
        public const string SubclassNamePrefix = "ToVersion";
        
        protected readonly SalaryCalculationDBContext context;

        public ReorganizationMain(SalaryCalculationDBContext context)
        {
            this.context = context;
        }

        public abstract void Run();
    }
}
