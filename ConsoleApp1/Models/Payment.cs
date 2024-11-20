﻿using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Enums;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Payment : SerializableObject<Payment>
    {
        [Required(ErrorMessage = "Payment ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Payment ID must be a positive integer.")]
        public int IdPayment { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment method is required.")]
        public PaymentMethod Method { get; set; }

        [Required(ErrorMessage = "Payment status is required.")]
        public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
        
        public Payment() { }
        
        public Payment(int idPayment, decimal amount, PaymentMethod method)
        {
            IdPayment = idPayment;
            Amount = amount;
            Method = method;
        }

        
        //METHODS
        public bool ProcessPayment()
        {
            if (Status != PaymentStatus.Pending)
            {
                Console.WriteLine($"Payment {IdPayment} is already {Status}. Cannot process again.");
                return false;
            }
            Status = PaymentStatus.Completed;
            Console.WriteLine($"Payment {IdPayment} of {Amount:C} has been processed successfully using {Method}.");
            return true;
        }
        
        public bool RefundPayment()
        {
            if (Status != PaymentStatus.Completed)
            {
                Console.WriteLine($"Payment {IdPayment} is not completed and cannot be refunded.");
                return false;
            }
            Status = PaymentStatus.Refunded;
            Console.WriteLine($"Payment {IdPayment} of {Amount:C} has been refunded.");
            return true;
        }
        
        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
    
            var other = (Payment)obj;
            return IdPayment == other.IdPayment && Amount == other.Amount && Method == other.Method;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdPayment, Amount, Method);
        }

        public override string ToString()
        {
            return $"Payment [ID: {IdPayment}, Amount: {Amount:C}, Method: {Method}, Status: {Status}]";
        }
    }
}