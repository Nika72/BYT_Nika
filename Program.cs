using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using ConsoleApp1.Services;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load all extents
            ExtentManager.LoadAllExtents();

            // Add fake data for all models
            AddFakeData();

            // Perform example operations
            PerformExampleOperations();

            // Save all extents
            ExtentManager.SaveAllExtents();
        }

        static void AddFakeData()
        {
            // Add Members
            SerializableObject<Member>.AddInstance(new Member(1, 50) { Email = "member1@example.com" });
            SerializableObject<Member>.AddInstance(new Member(2, 30) { Email = "member2@example.com" });

            // Add NonMembers
            SerializableObject<NonMember>.AddInstance(new NonMember { Id = 1 });
            SerializableObject<NonMember>.AddInstance(new NonMember { Id = 2 });

            // Add Tables
            SerializableObject<Table>.AddInstance(new Table(1, 4, "Round"));
            SerializableObject<Table>.AddInstance(new Table(2, 6, "Rectangle"));

            // Add Chefs
            SerializableObject<Chef>.AddInstance(new Chef(1, "Italian"));
            SerializableObject<Chef>.AddInstance(new Chef(2, "French"));

            // Add Executive Chefs
            SerializableObject<ExecutiveChef>.AddInstance(new ExecutiveChef(3, "Japanese", 15));
            SerializableObject<ExecutiveChef>.AddInstance(new ExecutiveChef(4, "Indian", 10));

            // Add Sous Chefs
            SerializableObject<SousChef>.AddInstance(new SousChef(5, "Mexican", "Spicy Specialist", 4));
            SerializableObject<SousChef>.AddInstance(new SousChef(6, "Mediterranean", "Seafood Specialist", 5));

            // Add Dishes
            SerializableObject<Dish>.AddInstance(new Dish
            {
                IdDish = 1,
                Name = "Pizza Margherita",
                Cuisine = "Italian",
                Price = 12.99m,
                IsVegetarian = true,
                IsVegan = false,
                Ingredients = new List<string> { "Tomato", "Mozzarella", "Basil" }
            });
            SerializableObject<Dish>.AddInstance(new Dish
            {
                IdDish = 2,
                Name = "Ratatouille",
                Cuisine = "French",
                Price = 9.99m,
                IsVegetarian = true,
                IsVegan = true,
                Ingredients = new List<string> { "Eggplant", "Zucchini", "Tomato" }
            });

            // Add Reservations
            SerializableObject<Reservation>.AddInstance(new Reservation(1, DateTime.Now.AddDays(1))
            {
                ReservedTable = new Table(1, 4, "Round")
            });

            // Add Orders
            var order = new Order { IdOrder = 1 };
            var pizzaDish = new Dish
            {
                IdDish = 1,
                Name = "Pizza Margherita",
                Cuisine = "Italian",
                Price = 12.99m
            };
            order.AddItem(pizzaDish, 2);
            SerializableObject<Order>.AddInstance(order);

            // Add Payments
            SerializableObject<Payment>.AddInstance(new Payment(1, 21.49m, Enums.PaymentMethod.Card));

            // Add Managers
            SerializableObject<Manager>.AddInstance(new Manager(1, Enums.ManagerLevel.MidLevel));
            SerializableObject<Manager>.AddInstance(new Manager(2, Enums.ManagerLevel.TopLevel));

            // Add Waiters
            SerializableObject<Waiter>.AddInstance(new Waiter(1));
            SerializableObject<Waiter>.AddInstance(new Waiter(2));

            // Add Vales
            SerializableObject<Vale>.AddInstance(new Vale(1, "Entrance A"));
            SerializableObject<Vale>.AddInstance(new Vale(2, "Entrance B"));

            Console.WriteLine("Fake data added for all models.");
        }

        static void PerformExampleOperations()
        {
            // Perform example operations with the fake data
            var member = new Member(3, 5) { Email = "example@test.com" };
            var dishes = new[]
            {
                new Dish { IdDish = 3, Name = "Sushi Roll", Price = 18.99m },
                new Dish { IdDish = 4, Name = "Pasta Carbonara", Price = 14.49m }
            };

            SerializableObject<Member>.AddInstance(member);

            var order = member.PlaceOrder(dishes);
            Console.WriteLine($"Order ID: {order.IdOrder} has been placed with {order.TotalItems} items totaling {order.TotalAmount:C}.");
            
            
            //////////////////////////////////////////////////////////////////
            /*
            // Create Chef instance and test ICuisineSpecialist implementation
            Chef chef = new Chef(1, "Italian");
            chef.AssignTask("Prepare pasta dishes");
            chef.DevelopCuisine(); // From ICuisineSpecialist

            Console.WriteLine(chef);

            // Create ExecutiveChef instance and test IKitchenManager implementation
            ExecutiveChef executiveChef = new ExecutiveChef(2, "French", 15);
            executiveChef.AssignTask("Supervise dinner service");
            executiveChef.DevelopCuisine(); // From ICuisineSpecialist (inherited)
            executiveChef.ManageKitchen();  // From IKitchenManager

            executiveChef.OverseeKitchen();
            executiveChef.TrainSousChef(new SousChef(3, "French", "Pastry", 4));

            Console.WriteLine(executiveChef);

            // Create SousChef instance and call its specific methods
            SousChef sousChef = new SousChef(3, "French", "Pastry", 4);
            sousChef.PrepareSpecials();
            sousChef.AssistHeadChef(executiveChef);

            Console.WriteLine(sousChef);

            // Testing polymorphism with interfaces
            ICuisineSpecialist cuisineSpecialist = chef;
            cuisineSpecialist.DevelopCuisine(); // Chef's implementation

            IKitchenManager kitchenManager = executiveChef;
            kitchenManager.ManageKitchen(); // ExecutiveChef's implementation

            Console.WriteLine("Tests completed successfully.");
            
            /////////////////////////////////////////////////////////////
            // Create some dishes
            Dish pizza = new Dish { IdDish = 1, Name = "Pizza", Price = 12.99m };
            Dish pasta = new Dish { IdDish = 2, Name = "Pasta", Price = 8.99m };

            // Create an order
            Order order = new Order { IdOrder = 101 };

            // Add items to the order
            order.AddItem(pizza, 2);  // 2 Pizzas
            order.AddItem(pasta, 1);  // 1 Pasta

            // Try adding the same pizza with the same quantity (should not be added again)
            order.AddItem(pizza, 2);  // Duplicate entry

            // Output total
            Console.WriteLine($"Total Order Amount: {order.TotalAmount:C}");

            // Output the details of the order
            Console.WriteLine("Order details:");
            foreach (var orderDish in order.OrderDishes)
            {
                orderDish.DisplayOrderDish();  // Display each dish's info
            }

            
            */
        }
    }
}