using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /* Базовый класс для хранимых сущностей */
    public abstract class BaseEntity
    {
        public int ID { get; set; }

        /*Запись активна или удалена*/
        [Required]
        public Boolean Active { get; set; }

        /*Дата создания записи*/
        [Required]
        public DateTime InsertDate { get; set; }

        /*Дата последнего обновления записи*/
        [Required]
        public DateTime UpdateDate { get; set; }
    }
}
