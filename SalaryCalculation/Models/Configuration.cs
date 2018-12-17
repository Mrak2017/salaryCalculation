using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /*Системные настройки*/
    public class Configuration : BaseEntity
    {
        /*Название*/
        [Required]
        public string Name { get; set; }

        /*Значение*/
        [Required]
        public string Value { get; set; }
    }
}
