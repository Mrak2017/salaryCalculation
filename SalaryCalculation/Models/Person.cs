using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        /*Базовая ставка зарплаты*/
        public Nullable<decimal> BaseSallaryPart { get; set; }

        /*Дата увольнения*/
        public Nullable<DateTime> EndDate { get; set; }

        /*Список групп в которые когда либо входил сотрудник*/
        public List<Person2Group> Groups { get; set; }

        /*Ссылка на положение в структуре компании*/
        public OrganizationStructureItem OrgStructure { get; set; }
    }
}
