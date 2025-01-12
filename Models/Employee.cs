using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models {
    public class Employee : SerializableObject<Employee>
    {
        [Required(ErrorMessage = "Employee ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Employee ID must be a positive integer.")]
        public int IdEmployee { get; set; }

        [Required(ErrorMessage = "Date of hiring is required.")]
        [DataType(DataType.Date)]
        public DateTime DateOfHiring { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfLeaving { get; set; } 

        [Required(ErrorMessage = "Department is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Department must be between 2 and 100 characters.")]
        public string Department { get; set; }

        
        
        
        //METHODS
        public int GetEmployedTime()
        {
            DateTime endDate = DateOfLeaving ?? DateTime.Now;
            return (endDate - DateOfHiring).Days;
        }
        
        //OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not Employee other)
                return false;

            return IdEmployee == other.IdEmployee && DateOfHiring == other.DateOfHiring;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdEmployee, DateOfHiring);
        }

        public override string ToString()
        {
            return $"Employee(IdEmployee={IdEmployee}, DateOfHiring={DateOfHiring.ToShortDateString()})";
        }
    }
}