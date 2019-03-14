using System;
using System.ComponentModel.DataAnnotations;

namespace SalaryCalculation.Models
{
    /*Тип группы*/
    public enum GroupType
    {
        Employee,
        Manager,
        Salesman
    }

    /* Отношение сотрудника к группе */
    public class Person2Group : BaseEntity
    {
        /*Сотрудник*/
        public Person Person { get; set; }

        /*Группа*/
        [Required]
        public GroupType GroupType { get; set; }

        /*Дата начала нахождения в группе*/
        [Required]
        public DateTime PeriodStart { get; set; }

        /*Дата окончания нахождения в группе*/
        public Nullable<DateTime> PeriodEnd { get; set; }
    }
}
