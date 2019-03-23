using SalaryCalculation.Models;
using System;
using System.Linq;

namespace SalaryCalculation.RestControllers.DTO
{
    public class PersonDTO
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string CurrentGroup { get; set; }

        public Person2GroupDTO[] Groups { get; set; } 

        public decimal? BaseSalaryPart { get; set; }

        public PersonDTO CurrentChief { get; set; }

        public decimal? CurrentSalary { get; set; }

        public PersonDTO()
        {

        }

        public PersonDTO(Person person, Person2Group[] groups, Person chief = null, decimal? currentSalary = null)
        {
            Id = person.ID;
            Login = person.Login;
            Password = person.Password;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            StartDate = person.StartDate;
            BaseSalaryPart = person.BaseSalaryPart;
            CurrentSalary = currentSalary;

            Person2Group currentGroup = Array.Find(groups, group => 
                group.PeriodStart <= DateTime.Today 
                && (group.PeriodEnd == null || group.PeriodEnd >= DateTime.Today));
            if (currentGroup != null)
            {
                CurrentGroup = currentGroup.GroupType.ToString();
            }

            Groups = groups.Select(group => new Person2GroupDTO(group)).ToArray();

            if (chief != null)
            {
                CurrentChief = new PersonDTO(chief, new Person2Group[0]);
            }
        }
    }
}
