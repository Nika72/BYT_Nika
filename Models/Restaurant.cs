using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Restaurant : SerializableObject<Restaurant>
    {
        [Required(ErrorMessage = "Restaurant name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Restaurant name must be between 2 and 100 characters.")]
        public string Name { get; set; }
        
        private int _maxCapacity;
        
        private readonly List<Menu> _menus = new List<Menu>(); 
        public IReadOnlyList<Menu> Menus => _menus.AsReadOnly();

        [Range(1, int.MaxValue, ErrorMessage = "Max capacity must be a positive integer.")]
        public int MaxCapacity
        {
            get { return _maxCapacity; }
            set
            {
                if (value <= 0) throw new ArgumentException("Max capacity must be greater than zero.");
                _maxCapacity = value;
            }
        }
        
        public List<Table> Tables { get; private set; } = new List<Table>();

        public Restaurant(){}
        public Restaurant(string name, int maxCapacity)
        {
            Name = name;
            MaxCapacity = maxCapacity;
        }

        
        //methods
        public int GetNumberOfTables()
        {
            return Tables.Count;
        }
        
        public void AddTable(Table table)
        {
            if (Tables.Count >= MaxCapacity)
                throw new InvalidOperationException($"Cannot add more tables. Max capacity of {MaxCapacity} reached.");
            Tables.Add(table);
            Console.WriteLine($"Table with ID {table.IdTable} added to restaurant '{Name}'.");
        }

        
        public bool RemoveTable(int tableId)
        {
            var table = Tables.Find(t => t.IdTable == tableId);
            if (table != null)
            {
                Tables.Remove(table);
                Console.WriteLine($"Table with ID {table.IdTable} removed from restaurant '{Name}'.");
                return true;
            }
            Console.WriteLine($"Table with ID {tableId} not found in restaurant '{Name}'.");
            return false;
        }
        
        public void AddMenu(Menu menu)
        {
            if (menu == null) throw new ArgumentNullException(nameof(menu), "Menu cannot be null.");
            if (_menus.Contains(menu)) return;

            _menus.Add(menu);
            menu.SetRestaurant(this); // Reverse connection
            Console.WriteLine($"Menu '{menu.Name}' added to restaurant '{Name}'.");
        }
        public bool RemoveMenu(string menuName)
        {
            var menu = _menus.Find(m => m.Name == menuName);
            if (menu != null)
            {
                _menus.Remove(menu);
                menu.SetRestaurant(null); // Clear reverse connection
                Console.WriteLine($"Menu '{menu.Name}' removed from restaurant '{Name}'.");
                return true;
            }

            Console.WriteLine($"Menu '{menuName}' not found in restaurant '{Name}'.");
            return false;
        }

        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
    
            var other = (Restaurant)obj;
            return Name == other.Name && MaxCapacity == other.MaxCapacity;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, MaxCapacity);
        }

        public override string ToString()
        {
            return $"Restaurant [Name: {Name}, Max Capacity: {MaxCapacity}, Tables Count: {Tables.Count}]";
        }
    }
}