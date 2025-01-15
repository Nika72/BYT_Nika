using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Vale : SerializableObject<Vale>, IVale
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

        public void ParkCar(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.car))
            {
                Console.WriteLine("Unable to park. Customer information or car details are missing.");
                return;
            }

            Console.WriteLine($"Vale ID: {IdVale} has parked the car '{customer.car}' for Customer ID: {customer.IdCustomer}.");
        }

        public void RetrieveCar(Customer customer)
        {
            if (customer == null || string.IsNullOrWhiteSpace(customer.car))
            {
                Console.WriteLine("Unable to retrieve. Customer information or car details are missing.");
                return;
            }

            Console.WriteLine($"Vale ID: {IdVale} has retrieved the car '{customer.car}' for Customer ID: {customer.IdCustomer}.");
        }


        public void Stand()
        {
            Console.WriteLine($"Vale ID: {IdVale} is standing by, ready for the next task.");
        }
    }
}