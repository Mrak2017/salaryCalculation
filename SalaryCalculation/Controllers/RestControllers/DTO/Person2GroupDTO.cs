using SalaryCalculation.Models;
using System;

namespace SalaryCalculation.Controllers.RestControllers.DTO
{
    public class Person2GroupDTO
    {
        public int Id { get; set; }

        public string GroupType { get; set; }

        public DateTime PeriodStart { get; set; }

        public DateTime? PeriodEnd { get; set; }

        public Person2GroupDTO()
        {
        }

        public Person2GroupDTO(Person2Group group)
        {
            Id = group.ID;
            GroupType = group.GroupType.ToString();
            PeriodStart = group.PeriodStart;
            PeriodEnd = group.PeriodEnd;
        }
    }
}
