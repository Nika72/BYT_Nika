using System;
using System.ComponentModel.DataAnnotations;

namespace ConsoleApp1.Models
{
    public class ExecutiveSousChef
    {
        public SousChef SousChefRole { get; private set; }
        public ExecutiveChef ExecutiveChefRole { get; private set; }

   
        [Required(ErrorMessage = "Cuisine type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Cuisine type must be between 3 and 50 characters.")]
        public string CuisineType { get; private set; }

        
        public ExecutiveSousChef(string cuisineType, SousChef sousChef, ExecutiveChef executiveChef)
        {
            CuisineType = cuisineType ?? throw new ArgumentNullException(nameof(cuisineType), "Cuisine type cannot be null.");
            SousChefRole = sousChef ?? throw new ArgumentNullException(nameof(sousChef), "SousChef cannot be null.");
            ExecutiveChefRole = executiveChef ?? throw new ArgumentNullException(nameof(executiveChef), "ExecutiveChef cannot be null.");
        }

       
        public void PrepareSpecials()
        {
            SousChefRole.PrepareSpecials();
        }

        public void AssistExecutiveChef()
        {
            SousChefRole.AssistHeadChef(ExecutiveChefRole);
        }

   
        public void OverseeKitchen()
        {
            ExecutiveChefRole.OverseeKitchen();
        }

        public void ApproveMenuChanges(Menu menu)
        {
            ExecutiveChefRole.ApproveMenuChanges(menu);
        }

        public void TrainSousChef(Chef sousChef)
        {
            ExecutiveChefRole.TrainSousChef(sousChef);
        }

     
        public void DisplayRoles()
        {
            Console.WriteLine($"Executive Sous Chef Role ({CuisineType} Cuisine)");
            Console.WriteLine($"- Sous Chef Role: Speciality - {SousChefRole.Speciality}, Supervision Level - {SousChefRole.SupervisionLevel}");
            Console.WriteLine($"- Executive Chef Role: Kitchen Experience - {ExecutiveChefRole.KitchenExperience} years");
        }
    }
}
