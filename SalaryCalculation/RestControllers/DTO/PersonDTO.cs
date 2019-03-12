using SalaryCalculation.Models;
using System;

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

        public decimal? BaseSalaryPart { get; set; }

        public PersonDTO()
        {

        }
    }
}
