using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Enums;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Customer : SerializableObject<Customer>
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Customer ID must be a positive integer.")]
        public int IdCustomer { get; set; }
        

        private readonly List<Order> _orders = new();
        public IReadOnlyList<Order> Orders => _orders.AsReadOnly();
        public string car{ get; set; }
        public object CurrentRole { get; private set; } // Dynamic role: Member or NonMember
        
        private readonly Dictionary<int, Reservation> _reservations = new(); 
        public IReadOnlyDictionary<int, Reservation> Reservations => _reservations; 

        public Customer(int idCustomer, string Car, object initialRole)
        {
            IdCustomer = idCustomer;
            
            car = Car;
            SwitchRole(initialRole);
        }

        public Customer()
        {
            
        }
        

        public void AddOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (!_orders.Contains(order))
            {
                _orders.Add(order);
                order.SetCustomer(this);
            }
        }
        
        public void SwitchRole(object newRole)
        {
            if (newRole is Member || newRole is NonMember)
            {
                CurrentRole = newRole;
                Console.WriteLine($"Customer {IdCustomer} switched to role: {newRole.GetType().Name}.");
            }
            else
            {
                throw new ArgumentException("Invalid role type. Must be Member or NonMember.", nameof(newRole));
            }
        }


        public void RemoveOrder(Order order)
        {
            if (order == null) return;
            if (_orders.Contains(order))
            {
                _orders.Remove(order);
                order.SetCustomer(null);
            }
        }
        //METHODS
        public Order PlaceOrder(Dish[] dishes)
        {
            if (dishes == null || dishes.Length == 0)
            {
                throw new ArgumentException("At least one dish must be ordered.");
            }

            if (CurrentRole is Member member)
            {
                Console.WriteLine($"Member {IdCustomer} placing an order with rewards.");
                member.PlaceOrder(dishes); 
            }
            else if (CurrentRole is NonMember)
            {
                Console.WriteLine($"NonMember {IdCustomer} placing an order without rewards.");
            }
            else
            {
                throw new InvalidOperationException("Unknown role. Cannot place order.");
            }

            Order order = new Order { IdOrder = Order.Instances.Count + 1 };
            foreach (var dish in dishes)
            {
                int quantity = 1;
                order.AddItem(dish, quantity);
            }

            Order.AddInstance(order);

            Console.WriteLine($"Order {order.IdOrder} placed by Customer {IdCustomer} with {dishes.Length} dishes.");
            return order;
        }


        public bool MakePayment(Order order, PaymentMethod paymentMethod)
        {
            if (order == null)
            {
                throw new ArgumentNullException(nameof(order), "Order cannot be null.");
            }

            if (CurrentRole is Member member && member.CreditPoints > 0)
            {
                Console.WriteLine($"Member {IdCustomer} used credits for payment.");
                member.UseCredits((int)order.CalculateTotal() / 10);
            }

            decimal amount = order.CalculateTotal();

            Payment payment = new Payment
            {
                IdPayment = Payment.Instances.Count + 1,
                Amount = amount,
                Method = paymentMethod
            };

            bool paymentSuccess = payment.ProcessPayment();
            if (paymentSuccess)
            {
                Payment.AddInstance(payment);
                Console.WriteLine(
                    $"Payment of {payment.Amount} for Order {order.IdOrder} completed by Customer {IdCustomer}.");
                return true;
            }
            else
            {
                Console.WriteLine($"Payment failed for Order {order.IdOrder} by Customer {IdCustomer}.");
                return false;
            }
        }
        //qualified association Customer and Order
        public void AddReservation(Reservation reservation) 
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation), "Reservation cannot be null.");
            if (_reservations.ContainsKey(reservation.IdReservation))
            {
                Console.WriteLine($"Reservation {reservation.IdReservation} already exists for Customer {IdCustomer}.");
                return;
            }
            _reservations.Add(reservation.IdReservation, reservation);
            reservation.SetCustomer(this); 
            Console.WriteLine($"Reservation {reservation.IdReservation} added to Customer {IdCustomer}.");
        }

        public void RemoveReservation(int reservationId)
        {
            if (_reservations.Remove(reservationId, out var reservation))
            {
                reservation.RemoveCustomer(); 
                Console.WriteLine($"Reservation {reservationId} removed from Customer {IdCustomer}.");
            }
            else
            {
                Console.WriteLine($"Reservation {reservationId} not found for Customer {IdCustomer}.");
            }
        }
        
        //OVERRIDES
        public override bool Equals(object obj)
        {
            if (obj is not Customer other)
                return false;

            return IdCustomer == other.IdCustomer;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdCustomer);
        }

        public override string ToString()
        {
            return $"Customer(IdCustomer={IdCustomer})";
            
        }
    }
}