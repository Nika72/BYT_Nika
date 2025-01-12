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
    
    public List<OrderDish> OrderDishes { get; private set; } = new List<OrderDish>();
    public Customer Customer { get; private set; }  
    
    private Payment _payment; 

    public int TotalItems => OrderDishes.Sum(orderDish => orderDish.Quantity);  
    
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
        {
            throw new ArgumentNullException(nameof(dish), "Dish cannot be null.");
        }
        
        var existingOrderDish = OrderDishes.FirstOrDefault(od => od.Dish.Equals(dish) && od.Quantity == quantity);
        
        if (existingOrderDish != null)
        {
            Console.WriteLine($"Dish '{dish.Name}' with quantity {quantity} is already added to Order {IdOrder}.");
        }
        else
        {
            var orderDish = new OrderDish(dish, quantity);  
            OrderDishes.Add(orderDish);  
            Console.WriteLine($"Dish '{dish.Name}' (Quantity: {quantity}) added to Order {IdOrder}. Current total: {TotalAmount:C}");
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
        return OrderDishes.Sum(orderDish => orderDish.TotalPrice);  
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
