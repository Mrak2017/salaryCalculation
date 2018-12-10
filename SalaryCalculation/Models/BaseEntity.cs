using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /* Базовый класс для хранимых сущностей */
    public class BaseEntity
    {
        public int ID { get; set; }
        public Boolean Active { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
