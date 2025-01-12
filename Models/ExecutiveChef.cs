using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Interfaces;

namespace ConsoleApp1.Models
{
    public class ExecutiveChef : Chef, IKitchenManager
    {
        [Required(ErrorMessage = "Kitchen experience is required.")]
        [Range(1, 50, ErrorMessage = "Kitchen experience must be between 1 and 50 years.")]
        public int KitchenExperience { get; set; } 
        
        public ExecutiveChef(int idChef, string cuisineType, int kitchenExperience)
            : base(idChef, cuisineType)
        {
            KitchenExperience = kitchenExperience;
        }
        
        public ExecutiveChef() {}
        
        public void ManageKitchen()
        {
            Console.WriteLine($"Executive Chef {IdChef} is managing the kitchen with {KitchenExperience} years of experience.");
        }
        
        //METHODS
        public void OverseeKitchen()
        {
            Console.WriteLine($"Executive Chef {IdChef} is overseeing the kitchen operations.");
        }
        
        public void ApproveMenuChanges(Menu menu)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu), "Menu cannot be null.");
            }

            Console.WriteLine($"Executive Chef {IdChef} has approved changes to the {menu.Name} menu.");
        }
        
        public void TrainSousChef(Chef sousChef)
        {
            if (sousChef == null)
            {
                throw new ArgumentNullException(nameof(sousChef), "Sous Chef cannot be null.");
            }

            Console.WriteLine($"Executive Chef {IdChef} is training Sous Chef with ID {sousChef.IdChef} in {sousChef.CuisineType} cuisine.");
        }
        
        //OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not ExecutiveChef other)
                return false;

            return IdChef == other.IdChef && CuisineType == other.CuisineType && KitchenExperience == other.KitchenExperience;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdChef, CuisineType, KitchenExperience);
        }

        public override string ToString()
        {
            return $"ExecutiveChef(IdChef={IdChef}, CuisineType={CuisineType}, KitchenExperience={KitchenExperience})";
        }
    }
}