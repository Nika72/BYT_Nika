using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Dish : SerializableObject<Dish>
    {
        [Required(ErrorMessage = "Dish ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Dish ID must be a positive integer.")]
        public int IdDish { get; set; }

        [Required(ErrorMessage = "Dish name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Dish name must be between 2 and 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cuisine type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Cuisine type must be between 3 and 50 characters.")]
        public string Cuisine { get; set; }

        public bool IsVegetarian { get; set; }

        public bool IsVegan { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0.01, 1000.00, ErrorMessage = "Price must be between $0.01 and $1000.")]
        public decimal Price { get; set; }

        public decimal DiscountedPrice => Price - (Price * VatPercentage);
        public decimal VatPercentage { get; set; } = 0.2m;
        public decimal PriceAfterTax => Price * (1 + VatPercentage);

        [Required(ErrorMessage = "Ingredients list cannot be empty.")]
        [MinLength(1, ErrorMessage = "At least one ingredient is required.")]
        public List<string> Ingredients { get; set; } = new List<string>();

        public Menu Menu { get; private set; }

        public Dish() { }

        public void SetMenu(Menu menu)
        {
            Menu = menu;
            Console.WriteLine(menu == null
                ? $"Dish '{Name}' (ID: {IdDish}) is no longer associated with any menu."
                : $"Dish '{Name}' (ID: {IdDish}) is now part of menu '{menu.Name}'.");
        }

        // METHODS
        public static Dish CreateDish(int idDish, string name, string cuisine, bool isVegetarian, bool isVegan,
            decimal price, List<string> ingredients)
        {
            if (ingredients == null || ingredients.Count == 0)
                
                throw new ArgumentException("At least one ingredient is required to create a dish.");

            
            Dish dish = new Dish
            {
                IdDish = idDish,
                Name = name,
                Cuisine = cuisine,
                IsVegetarian = isVegetarian,
                IsVegan = isVegan,
                Price = price,
                Ingredients = new List<string>(ingredients)
            };

            AddInstance(dish);
            Console.WriteLine($"Dish '{name}' created with ID {idDish}.");
            return dish;
        }

        public void ChangeDish(string? name = null, string? cuisine = null, bool? isVegetarian = null,
            bool? isVegan = null, decimal? price = null, List<string>? ingredients = null)
        {
            if (name != null) Name = name;
            if (cuisine != null) Cuisine = cuisine;
            if (isVegetarian.HasValue) IsVegetarian = isVegetarian.Value;
            if (isVegan.HasValue) IsVegan = isVegan.Value;
            if (price.HasValue) Price = price.Value;
            if (ingredients != null && ingredients.Count > 0) Ingredients = new List<string>(ingredients);

            Console.WriteLine($"Dish '{Name}' with ID {IdDish} has been updated.");
        }

        public static bool RemoveDish(int idDish)
        {
            Dish? dishToRemove = Instances.Find(d => d.IdDish == idDish);
            if (dishToRemove != null)
            {
                dishToRemove.SetMenu(null);

                Instances.Remove(dishToRemove);
                Console.WriteLine($"Dish with ID {idDish} has been removed.");
                return true;
            }

            Console.WriteLine($"Dish with ID {idDish} not found.");
            return false;
        }

        public void AddIngredient(string ingredient)
        {
            if (string.IsNullOrWhiteSpace(ingredient))
                throw new ArgumentException("Ingredient cannot be null or empty.");
            Ingredients.Add(ingredient);
        }

        // OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not Dish other)
                return false;

            return IdDish == other.IdDish && Name == other.Name;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdDish, Name);
        }

        public override string ToString()
        {
            return $"Dish(IdDish={IdDish}, Name={Name}, Price={Price:C}, Discounted Price={DiscountedPrice:C}, Price After Tax={PriceAfterTax:C})";
        }
    }
}
