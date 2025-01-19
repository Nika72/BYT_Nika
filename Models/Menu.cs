using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Menu : SerializableObject<Menu>
    {
        [Required(ErrorMessage = "Menu name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Menu name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Menu type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Menu type must be between 3 and 50 characters.")]
        public string MenuType { get; set; }

        public List<string> AvailableLanguages => new List<string> { "English", "Polish", "Turkish" };

        private readonly List<Dish> _dishes = new List<Dish>();
        public IReadOnlyList<Dish> Dishes => _dishes.AsReadOnly();
        public Restaurant Restaurant { get; private set; }

        public void SetRestaurant(Restaurant restaurant)
        {
            Restaurant = restaurant;
            Console.WriteLine(restaurant == null
                ? $"Menu '{Name}' is no longer associated with any restaurant."
                : $"Menu '{Name}' is now associated with restaurant '{restaurant.Name}'.");
        }


        public Menu() { }

        public Menu(string name, string menuType)
        {
            Name = name;
            MenuType = menuType;
        }

       
        public void ChangeMenu(string? newName = null, string? newMenuType = null)
        {
            if (!string.IsNullOrEmpty(newName)) Name = newName;
            if (!string.IsNullOrEmpty(newMenuType)) MenuType = newMenuType;

            Console.WriteLine($"Menu '{Name}' has been updated.");
        }

        public int GetNumberOfPositions() => _dishes.Count;

        // Add Dish with Reverse Connection
        public void AddDish(Dish dish)
        {
            if (dish == null) throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
            if (_dishes.Contains(dish)) return;

            _dishes.Add(dish);
            dish.SetMenu(this); // Reverse connection
            Console.WriteLine($"Dish '{dish.Name}' added to the menu '{Name}'.");
        }

        // Remove Dish with Reverse Connection
        public bool RemoveDish(int dishId)
        {
            var dish = _dishes.Find(d => d.IdDish == dishId);
            if (dish != null)
            {
                _dishes.Remove(dish);
                dish.SetMenu(null); 
                Console.WriteLine($"Dish '{dish.Name}' removed from the menu '{Name}'.");
                return true;
            }

            Console.WriteLine($"Dish with ID {dishId} not found in the menu '{Name}'.");
            return false;
        }

       
        public bool ModifyDish(int dishId, string newName, decimal newPrice)
        {
            var dish = _dishes.Find(d => d.IdDish == dishId);
            if (dish != null)
            {
                dish.ChangeDish(newName, price: newPrice);
                Console.WriteLine($"Dish '{dish.Name}' modified in the menu '{Name}'.");
                return true;
            }

            Console.WriteLine($"Dish with ID {dishId} not found in the menu '{Name}'.");
            return false;
        }

      
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            var other = (Menu)obj;
            return Name == other.Name && MenuType == other.MenuType;
        }

        public override int GetHashCode() => HashCode.Combine(Name, MenuType);

        public override string ToString() => $"Menu [Name: {Name}, Type: {MenuType}, Dishes Count: {_dishes.Count}]";
    }
}
