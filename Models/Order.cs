using System.ComponentModel.DataAnnotations;
using ConsoleApp1.Enums;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

public class Order : SerializableObject<Order>
{
    [Required(ErrorMessage = "Order ID is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "Order ID must be a positive integer.")]
    public int IdOrder { get; set; }

    [Required(ErrorMessage = "Timestamp is required.")]
    public DateTime TimeStamp { get; set; } = DateTime.Now;
    
    public decimal TotalAmount => CalculateTotal();  
    
    private readonly Dictionary<int, OrderDish> _orderDishes = new(); 
    public IReadOnlyDictionary<int, OrderDish> OrderDishes => _orderDishes;
    public Customer Customer { get; private set; }  
    
    private Payment _payment; 

    public int TotalItems => OrderDishes.Values.Sum(orderDish => orderDish.Quantity);
    
    private readonly List<Payment> _payments = new List<Payment>();
    public IReadOnlyList<Payment> Payments => _payments.AsReadOnly();

    
    public void SetCustomer(Customer customer)
    {
        if (Customer == customer) return;
        Customer?.RemoveOrder(this);
        Customer = customer;
        Customer?.AddOrder(this);
    }
    
    public void AddItem(Dish dish, int quantity)
    {
        if (dish == null)
            throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

        if (_orderDishes.ContainsKey(dish.IdDish))
        {
            // Update quantity if Dish already exists
            _orderDishes[dish.IdDish].Quantity += quantity;
            Console.WriteLine($"Updated quantity for Dish '{dish.Name}' in Order {IdOrder}.");
        }
        else
        {
            // Add new OrderDish entry
            var orderDish = new OrderDish(dish, quantity);
            _orderDishes[dish.IdDish] = orderDish;
            Console.WriteLine($"Dish '{dish.Name}' (Quantity: {quantity}) added to Order {IdOrder}.");
        }
    }
    public void RemoveItem(int dishId)
    {
        if (_orderDishes.Remove(dishId))
        {
            Console.WriteLine($"Dish with ID {dishId} removed from Order {IdOrder}.");
        }
        else
        {
            Console.WriteLine($"Dish with ID {dishId} not found in Order {IdOrder}.");
        }
    }

    public void AddPayment(decimal amount, PaymentMethod method)
    {
        if (amount <= 0) throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        var payment = new Payment
        {
            IdPayment = _payments.Count + 1,
            Amount = amount,
            Method = method,
            _order = this 
        };

        _payments.Add(payment);
        Console.WriteLine($"Payment {payment.IdPayment} added to Order {IdOrder}.");
    }

    public void RemovePayment(int paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.IdPayment == paymentId);
        if (payment != null)
        {
            _payments.Remove(payment);
            Console.WriteLine($"Payment {payment.IdPayment} removed from Order {IdOrder}.");
        }
    }

    public decimal CalculateTotal()
    {
        return OrderDishes.Values.Sum(orderDish => orderDish.TotalPrice);
    }

    public Payment Payment 
    {
        get => _payment;
        private set
        {
            if (_payment == value) return;

            _payment?.RemoveOrder(); 
            _payment = value;
            _payment?.SetOrder(this); 
        }
    }

    public void SetPayment(Payment payment) 
    {
        if (payment == null) throw new ArgumentNullException(nameof(payment));
        Payment = payment;
        Console.WriteLine($"Payment {payment.IdPayment} associated with Order {IdOrder}.");
    }

    public void RemovePayment() 
    {
        if (Payment != null)
        {
            var oldPayment = Payment;
            Payment = null;
            Console.WriteLine($"Payment {oldPayment.IdPayment} removed from Order {IdOrder}.");
        }
    }
    // OVERRIDES 
    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        var other = (Order)obj;
        return IdOrder == other.IdOrder && TimeStamp == other.TimeStamp && OrderDishes.SequenceEqual(other.OrderDishes);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IdOrder, TimeStamp, OrderDishes);
    }

    public override string ToString()
    {
        return $"Order(ID: {IdOrder}, TimeStamp: {TimeStamp}, Total Amount: {TotalAmount:C}, Payments: {_payments.Count})";
    }

}
