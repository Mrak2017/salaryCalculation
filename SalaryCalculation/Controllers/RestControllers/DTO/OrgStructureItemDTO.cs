using SalaryCalculation.Models;

namespace SalaryCalculation.Controllers.RestControllers.DTO
{
    public class OrgStructureItemDTO
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public OrgStructureItemDTO[] Children { get; set; }

        public OrgStructureItemDTO(Person person, OrgStructureItemDTO[] children = null)
        {
            PersonId = person.ID;
            FirstName = person.FirstName;
            MiddleName = person.MiddleName;
            LastName = person.LastName;
            Children = children;
        }
    }
}
