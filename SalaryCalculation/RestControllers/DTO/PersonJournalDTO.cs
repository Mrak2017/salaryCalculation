using SalaryCalculation.Models;
using System;

namespace SalaryCalculation.RestControllers.DTO
{
    public class PersonJournalDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CurrentGroup { get; set; }

        public decimal? BaseSalaryPart { get; set; }

        public PersonJournalDTO(Person person, GroupType? group)
        {
            Id = person.ID;
            Login = person.Login;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            StartDate = person.StartDate;
            EndDate = person.EndDate;
            BaseSalaryPart = person.BaseSalaryPart;
            if (group != null)
            {
                CurrentGroup = group.ToString();
            }
        }
    }
}
