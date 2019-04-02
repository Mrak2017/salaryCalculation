using SalaryCalculation.Models;

namespace SalaryCalculation.Controllers.RestControllers.DTO
{
    public class ComboBoxItemDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        
        public ComboBoxItemDTO(Person person)
        {
            Id = person.ID;
            string middleName = person.MiddleName != null ? " " + person.MiddleName : "";
            Name = person.LastName + " " + person.FirstName + middleName;
        }
    }
}
