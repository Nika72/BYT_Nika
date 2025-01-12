using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Reservation : SerializableObject<Reservation>
    {
        [Required(ErrorMessage = "Reservation ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Reservation ID must be a positive integer.")]
        public int IdReservation { get; set; }

        [Required(ErrorMessage = "Date of reservation is required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date and time format.")]
        public DateTime DateOfReservation { get; set; }

        private Table _reservedTable;

        private Customer _customer; 

        // Reverse connection property
        public Table ReservedTable
        {
            get => _reservedTable;
            set
            {
                if (_reservedTable == value) return;

                _reservedTable?.RemoveReservation(this); 
                _reservedTable = value;
                _reservedTable?.AddReservation(this);    
            }
        }

        public Reservation() { }

        public Reservation(int idReservation, DateTime dateOfReservation)
        {
            IdReservation = idReservation;
            DateOfReservation = dateOfReservation;
        }

        public Customer Customer 
        {
            get => _customer;
            private set => _customer = value;
        }

        public void SetCustomer(Customer customer) 
        {
            if (_customer == customer) return;

            _customer?.RemoveReservation(IdReservation); 
        }

        public void RemoveCustomer() 
        {
            if (_customer != null)
            {
                var oldCustomer = _customer;
                _customer = null;
                Console.WriteLine($"Reservation {IdReservation} removed from Customer {oldCustomer.IdCustomer}.");
            }
        }
        public bool ReserveTable(Table table, int minCapacity, int maxCapacity)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table), "Table cannot be null.");

            if (table.NumberOfChairs < minCapacity || table.NumberOfChairs > maxCapacity)
            {
                Console.WriteLine($"Table {table.IdTable} does not meet capacity requirements.");
                return false;
            }

            if (DateOfReservation.Date < DateTime.Now.Date)
            {
                Console.WriteLine($"Cannot reserve table for past dates.");
                return false;
            }

            ReservedTable = table; 
            Console.WriteLine($"Reservation {IdReservation} confirmed for Table {table.IdTable}.");
            return true;
        }

        private bool CheckTableAvailability(Table table, DateTime reservationDate)
        {
            var existingReservations = Reservation.Instances;
            foreach (var reservation in existingReservations)
            {
                if (reservation.ReservedTable?.IdTable == table.IdTable &&
                    reservation.DateOfReservation.Date == reservationDate.Date)
                {
                    Console.WriteLine($"Table {table.IdTable} already reserved for {reservation.DateOfReservation}.");
                    return false;
                }
            }
            return true;
        }

        // Added Methods
        public void AddTable(Table table)
        {
            if (table == null) throw new ArgumentNullException(nameof(table));
            ReservedTable = table;
        }

        public void RemoveTable()
        {
            if (ReservedTable != null)
            {
                var oldTable = ReservedTable;
                ReservedTable = null;
                Console.WriteLine($"Reservation {IdReservation} removed from Table {oldTable.IdTable}.");
            }
        }

        public override string ToString()
        {
            return $"Reservation [ID: {IdReservation}, Date: {DateOfReservation}, Table ID: {ReservedTable?.IdTable}]";
        }
    }
}
