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
        /*Код (уникальный)*/
        [Required]
        public string Code { get; set; }

        /*Значение*/
        [Required]
        public string Value { get; set; }

        /*Описание*/
        public string Decription { get; set; }
    }
}
