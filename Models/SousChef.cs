using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Models;

namespace ConsoleApp1.Models
{
    public class SousChef : Chef
    {
        [Required(ErrorMessage = "Speciality is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Speciality must be between 3 and 50 characters.")]
        public string Speciality { get; set; }

        [Required(ErrorMessage = "Supervision level is required.")]
        [Range(1, 5, ErrorMessage = "Supervision level must be between 1 and 5.")]
        public int SupervisionLevel { get; set; } 
        
        public SousChef(int idChef, string cuisineType, string speciality, int supervisionLevel)
            : base(idChef, cuisineType)
        {
            Speciality = speciality;
            SupervisionLevel = supervisionLevel;
        }
        public SousChef() {}

        
        //METHODS
        public void PrepareSpecials()
        {
            Console.WriteLine($"Sous Chef {IdChef} ({Speciality} Specialist) is preparing today's specials.");
        }
        
        public void AssistHeadChef(ExecutiveChef headChef)
        {
            if (headChef == null)
            {
                throw new ArgumentNullException(nameof(headChef), "Head Chef cannot be null.");
            }

            Console.WriteLine($"Sous Chef {IdChef} is assisting Executive Chef {headChef.IdChef} in {headChef.CuisineType} cuisine.");
        }
        

        //OVERRIDES
        public bool Equals(SousChef other)
        {
            if (other == null) return false;
            return this.IdChef == other.IdChef && this.Speciality == other.Speciality;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as SousChef);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdChef, Speciality);
        }
    }
}