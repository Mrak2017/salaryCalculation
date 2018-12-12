using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    enum Group
    {
        Employee,
        Manager,
        Salesman
    }

    public class Person2Group : BaseEntity
    {
        /*Сотрудник*/
        [Required]
        public Person Person { get; set; }

    }
}
