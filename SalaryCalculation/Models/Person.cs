using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation.Models
{
    /* Сотрудник компании */
    public class Person : BaseEntity
    {
        /*Логин*/
        public string Login { get; set; }
        
        /*Пароль*/
        public string Password { get; set; }

        /*Фамилия*/
        public string LastName { get; set; }

        /*Имя*/
        public string FirstName { get; set; }

        /*Отчество*/
        public string MiddleName { get; set; }

        /*Дата устройства на работу*/
        public DateTime StartDate { get; set; }

        /*Дата увольнения*/
        public DateTime EndDate { get; set; }
    }
}
