using System.ComponentModel.DataAnnotations;
using ConsoleApp1;
using ConsoleApp1.Services;

namespace ConsoleApp1.Models
{
    public class Table : SerializableObject<Table>
    {
        [Required(ErrorMessage = "Table ID is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Table ID must be a positive integer.")]
        public int IdTable { get; set; }

        [Required(ErrorMessage = "Number of chairs is required.")]
        [Range(1, 20, ErrorMessage = "Number of chairs must be between 1 and 20.")]
        public int NumberOfChairs { get; set; }

        [Required(ErrorMessage = "Table type is required.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Table type must be between 3 and 50 characters.")]
        public string TableType { get; set; }

        private readonly List<Reservation> _reservations = new();
        public IReadOnlyList<Reservation> Reservations => _reservations.AsReadOnly();

        public Table() { }

        public Table(int idTable, int numberOfChairs, string tableType)
        {
            IdTable = idTable;
            NumberOfChairs = numberOfChairs;
            TableType = tableType;
        }

        // Existing Methods
        public override string ToString()
        {
            return $"Table {IdTable} - Type: {TableType}, Chairs: {NumberOfChairs}";
        }

        // Added Methods
        public void AddReservation(Reservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));
            if (_reservations.Contains(reservation)) return;

            _reservations.Add(reservation);
            if (reservation.ReservedTable != this)
            {
                reservation.AddTable(this); // Ensure reverse connection
            }
            Console.WriteLine($"Reservation {reservation.IdReservation} added to Table {IdTable}.");
        }

        public void RemoveReservation(Reservation reservation)
        {
            if (reservation == null) throw new ArgumentNullException(nameof(reservation));
            if (_reservations.Remove(reservation))
            {
                if (reservation.ReservedTable == this)
                {
                    reservation.RemoveTable(); // Ensure reverse disconnection
                }
                Console.WriteLine($"Reservation {reservation.IdReservation} removed from Table {IdTable}.");
            }
        }
    }
}
