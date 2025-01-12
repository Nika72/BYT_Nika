using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Person : SerializableObject<Person>
    {
        [Required(ErrorMessage = "Person ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Person ID must be a positive integer.")]
        public int IdPerson { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Birth date is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Person), nameof(ValidateBirthDate))]
        public DateTime BirthOfDate { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }
        
        public Person() { }

        public static ValidationResult? ValidateBirthDate(DateTime birthOfDate, ValidationContext context)
        {
            if (birthOfDate > DateTime.Now)
            {
                return new ValidationResult("Birth date cannot be in the future.");
            }
            return ValidationResult.Success;
        }
        
        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
    
            var other = (Person)obj;
            return IdPerson == other.IdPerson && FirstName == other.FirstName && LastName == other.LastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPerson, FirstName, LastName);
        }

        public override string ToString()
        {
            return $"Person [ID: {IdPerson}, Name: {FirstName} {LastName}, Birth Date: {BirthOfDate.ToShortDateString()}, Phone: {PhoneNumber}]";
        }
    }
}