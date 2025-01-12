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
        
        
        private readonly Dictionary<int, Reservation> _reservations = new(); 
        public IReadOnlyDictionary<int, Reservation> Reservations => _reservations; 

        
        public void AddOrder(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            if (!_orders.Contains(order))
            {
                _orders.Add(order);
                order.SetCustomer(this);
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