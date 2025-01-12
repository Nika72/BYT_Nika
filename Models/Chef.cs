using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Services;
//Test
namespace ConsoleApp1.Models
{
    public class Chef : SerializableObject<Chef>, ICuisineSpecialist, IKitchenManager
    {
        [Required(ErrorMessage = "Chef ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Chef ID must be a positive integer.")]
        public int IdChef { get; set; }

        [Required(ErrorMessage = "Cuisine type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Cuisine type must be between 3 and 50 characters.")]
        public string CuisineType { get; set; }
        
        
        public Chef(){}
        public Chef(int idChef, string cuisineType)
        {
            IdChef = idChef;
            CuisineType = cuisineType;
        }
        
        public void DevelopCuisine()
        {
            Console.WriteLine($"Chef {IdChef} is developing recipes for {CuisineType} cuisine.");
        }
        
        public void ManageKitchen()
        {
            Console.WriteLine($"Chef {IdChef} is managing the kitchen.");
        }
        
        //METHODS
        public void AssignTask(string task)
        {
            if (string.IsNullOrWhiteSpace(task))
            {
                throw new ArgumentException("Task cannot be null or empty.", nameof(task));
            }
            Console.WriteLine($"Chef {IdChef} (Cuisine: {CuisineType}) has been assigned the task: {task}");
        }
        
        public void AddDish(Dish dish, Menu menu)
        {
            if (dish == null || menu == null)
                throw new ArgumentNullException("Dish or Menu cannot be null.");

            menu.AddDish(dish);
            Console.WriteLine($"Chef {IdChef} added Dish {dish.Name} to Menu {menu.Name}.");
        }
        
        //OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not Chef other)
                return false;

            return IdChef == other.IdChef && CuisineType == other.CuisineType;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdChef, CuisineType);
        }

        public override string ToString()
        {
            return $"Chef(IdChef={IdChef}, CuisineType={CuisineType})";
        }
    }
}