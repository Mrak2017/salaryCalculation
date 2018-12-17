using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalaryCalculation.Models
{
    /*Положение сотрудника в иерархии*/
    public class OrganizationStructureItem : BaseEntity
    {
        /*Сотрудник*/
        [Required]
        public Person Person { get; set; }

        public int PersonId { get; set; }

        /*Ссылка на родителя в иерархии*/
        public int? ParentId { get; set; }

        /*По порядку, все айди вышестоящих сотрудников начиная с самого первого*/
        public ICollection<string> MaterializedPath { get; set; } /// заполняется на сохранении сущности сотрудника

        [ForeignKey("ParentId")]
        public virtual OrganizationStructureItem Parent { get; set; }

        public virtual ICollection<OrganizationStructureItem> Children { get; set; }
    }
}
