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
            this.Id = person.ID;
            this.Login = person.Login;
            this.FirstName = person.FirstName;
            this.MiddleName = person.MiddleName;
            this.LastName = person.LastName;
            this.StartDate = person.StartDate;
            this.EndDate = person.EndDate;
            this.BaseSalaryPart = person.BaseSalaryPart;
            if (group != null)
            {
                this.CurrentGroup = group.ToString();
            }
        }
    }
}
