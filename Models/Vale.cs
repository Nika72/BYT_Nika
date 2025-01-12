using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Vale : SerializableObject<Vale>
    {
        [Required(ErrorMessage = "Vale ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vale ID must be a positive integer.")]
        public int IdVale { get; set; }

        [Required(ErrorMessage = "Assigned location is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Assigned location must be between 3 and 100 characters.")]
        public string AssignedLocation { get; set; }

        public Vale(){}
        public Vale(int idVale, string assignedLocation)
        {
            IdVale = idVale;
            AssignedLocation = assignedLocation;
        }

        
        //METHODS
        public void DisplayValeInfo()
        {
            Console.WriteLine($"Vale ID: {IdVale}, Assigned Location: {AssignedLocation}");
        }
        
       
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj is Vale other)
            {
                return IdVale == other.IdVale && AssignedLocation == other.AssignedLocation;
            }
            return false;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(IdVale, AssignedLocation);
        }

        public override string ToString()
        {
            return $"Vale ID: {IdVale}, Assigned Location: {AssignedLocation}";
        }
    }
}