using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Member : SerializableObject<Member>
    {
        [Required(ErrorMessage = "Member ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Member ID must be a positive integer.")]
        public int IdMember { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }

        private int creditPoints;

        [Range(0, int.MaxValue, ErrorMessage = "Credit points cannot be negative.")]
        [XmlElement("CreditPoints")]
        public int CreditPoints
        {
            get => creditPoints; 
            set => creditPoints = Math.Max(0, value); 
        }
        
        public Member() { }
        public Member(int idMember, int initialCreditPoints = 0)
        {
            IdMember = idMember;
            CreditPoints = initialCreditPoints;
        }
        
        
        //METHODS
        public Order PlaceOrder(Dish[] dishes)
        {
            if (dishes == null || dishes.Length == 0)
            {
                throw new ArgumentException("At least one dish must be ordered.");
            }
            
            var order = new Order { IdOrder = Order.Instances.Count + 1 };
            foreach (var dish in dishes)
            {   
                int quantity = 1;
                order.AddItem(dish, quantity);
            }

            Order.AddInstance(order);
            CreditPoints++;
            Console.WriteLine($"Order {order.IdOrder} placed by Member {IdMember}. Credit points increased to {CreditPoints}.");
            return order;
        }

        public bool UseCredits(int pointsToUse)
        {
            if (pointsToUse <= 0)
            {
                Console.WriteLine("Points to use must be greater than zero.");
                return false;
            }
            
            if (pointsToUse > CreditPoints)
            {
                Console.WriteLine($"Insufficient credit points. Available: {CreditPoints}, Requested: {pointsToUse}");
                return false;
            }

            CreditPoints -= pointsToUse;
            Console.WriteLine($"{pointsToUse} credit points used. Remaining points: {CreditPoints}");
            return true;
        }
        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            var other = (Member)obj;
            return IdMember == other.IdMember && CreditPoints == other.CreditPoints && Email == other.Email;
        }
        
        public override int GetHashCode()
        {
            return HashCode.Combine(IdMember, CreditPoints, Email);
        }

        public override string ToString()
        {
            return $"Member(ID: {IdMember}, Email: {Email ?? "N/A"}, Credit Points: {CreditPoints})";
        }
    }
}
