using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /* Сотрудник компании */
    public class Person : BaseEntity
    {
        /*Логин*/
        [Required]
        public string Login { get; set; }

        /*Пароль*/
        [Required]
        public string Password { get; set; }

        /*Фамилия*/
        [Required]
        public string LastName { get; set; }

        /*Имя*/
        [Required]
        public string FirstName { get; set; }

        /*Отчество*/
        public string MiddleName { get; set; }

        /*Дата устройства на работу*/
        [Required]
        public DateTime StartDate { get; set; }

        /*Дата увольнения*/
        public Nullable<DateTime> EndDate { get; set; }
    }
}
