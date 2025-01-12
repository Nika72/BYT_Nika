using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Waiter : SerializableObject<Waiter>
    {
        [Required(ErrorMessage = "Waiter ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Waiter ID must be a positive integer.")]
        public int IdWaiter { get; set; }
        public List<Table> AssignedTables { get; private set; } = new List<Table>();

        public Waiter(){}
        public Waiter(int idWaiter)
        {
            IdWaiter = idWaiter;
        }

        
        //METHODS
        public void AssignTable(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table), "Table cannot be null.");
            }

            AssignedTables.Add(table);
            Console.WriteLine($"Table {table.IdTable} assigned to Waiter {IdWaiter}.");
        }
        
        public bool UnassignTable(int tableId)
        {
            var table = AssignedTables.Find(t => t.IdTable == tableId);
            if (table != null)
            {
                AssignedTables.Remove(table);
                Console.WriteLine($"Table {table.IdTable} unassigned from Waiter {IdWaiter}.");
                return true;
            }

            Console.WriteLine($"Table with ID {tableId} not found in Waiter {IdWaiter}'s assigned tables.");
            return false;
        }
        
        
        //OVERRIDES
        public override bool Equals(object? obj)
        {
            if (obj is Waiter other)
            {
                return IdWaiter == other.IdWaiter && AssignedTables.SequenceEqual(other.AssignedTables);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdWaiter, AssignedTables);
        }

        public override string ToString()
        {
            return $"Waiter ID: {IdWaiter}, Assigned Tables: {AssignedTables.Count}";
        }
    }
}